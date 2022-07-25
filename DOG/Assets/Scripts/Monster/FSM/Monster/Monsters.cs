using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Monster.Enums;

public class Monsters : MonoBehaviour, IHealth, IBattle
{
    protected Rigidbody2D rigid = null;
    protected SpriteRenderer sprite = null;
    protected Animator anim = null;
    
    public static bool isDead = false;

    public System.Action onHit = null;

    [Header("몬스터 AI 관련")]
    public MonsterCurrentState status = MonsterCurrentState.IDLE;
    protected Vector2 trackDirection = Vector2.zero;
    [SerializeField] protected float detectRadius = 5.0f;

    [Header("몬스터 기본스탯")]
    [SerializeField] protected float healthPoint = 100.0f;
    protected float maxHealthPoint = 100.0f;
    [SerializeField] protected int strength = 5;
    [SerializeField] protected float moveSpeed = 3.0f;
    protected float currentSpeed = 3.0f;


    // #################################### VARIABLES #####################################
    // ------------------------------------ TRACK ------------------------------------------

    // ------------------------------------ TARGET ------------------------------------------
    protected Vector2 target = new();

    // ------------------------------------ ATTACK ------------------------------------------
    [SerializeField] protected float attackRange = 1.5f;
    [SerializeField] protected float attackInterval = 3.0f;
    [SerializeField] protected float attackPower = 5.0f;
    [SerializeField] protected float defence = 1.0f;

    IEnumerator attack = null;

    public System.Action onHealthChange { get; set; }

    // ------------------------------------ PATROL ------------------------------------------
    float waitCounter = 0.0f;
    float waitTime = 2.0f;
    int waypointIndex = 0;
    public Transform[] waypoint = null;


    //################################## PROPERTIES ########################################
    public float HP
    {
        get => healthPoint;
        set 
        { 
            healthPoint = Mathf.Clamp(value, 0f, maxHealthPoint);
            onHealthChange?.Invoke();
        } 
    }

    public float MaxHP { get => maxHealthPoint; }

    public float AttackPower { get => attackPower; }

    public float Defence { get => defence; }

    public Vector2 TrackDirection { get => trackDirection; }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        attack = Attack();
    }

    private void FixedUpdate()
    {
        CheckStatus();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ChangeStatus(MonsterCurrentState.ATTACK);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ChangeStatus(MonsterCurrentState.TRACK);
        }
    }

    //################################## Method ########################################
    // -------------------------------  IDLE  ------------------------------------------
    void Idle()
    {
        if (Search())
        {
            ChangeStatus(MonsterCurrentState.TRACK);
            return;
        }
    }

    // -----------------------------  SEARCH  ------------------------------------------
    protected virtual bool Search()
    {
        bool result = false;

        Collider2D collider = Physics2D.OverlapCircle(this.transform.position, detectRadius, LayerMask.GetMask("Player"));

        if (collider)
        {
            target = collider.transform.position;
            result = true;
        }
        return result;
    }

    // -------------------------------  MOVE  ------------------------------------------
    protected virtual void Track()
    {
        if (!Search())
        {
            ChangeStatus(MonsterCurrentState.PATROL);
            return;
        }
        else
        {
            Move_Monster(currentSpeed);
        }
    }


    void Patrol()
    {
        if (Search())
        {
            ChangeStatus(MonsterCurrentState.TRACK);
            return;
        }

        if (Vector2.SqrMagnitude(transform.position - waypoint[waypointIndex].position) < 0.01f)
        {
            ChangeStatus(MonsterCurrentState.IDLE);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, waypoint[waypointIndex].position,
                  Time.fixedDeltaTime * moveSpeed);
        }
    }

    protected virtual void Move_Monster(float speed)
    {
        trackDirection = target - (Vector2)this.transform.position;
        rigid.position = Vector2.MoveTowards(rigid.position, target, speed * Time.fixedDeltaTime);
        SpriteFlip();
    }

    protected virtual void SpriteFlip()
    {
        var cross = Vector3.Cross(trackDirection, (Vector2)this.transform.up);
        if (Vector3.Dot(cross, transform.forward) < 0)
        {   // 왼쪽
            sprite.flipX = true;
        }
        else
        {   // 오른쪽
            sprite.flipX = false;
        }
    }

    // -------------------------------  ATTACK  ----------------------------------------
    protected IEnumerator Attack()
    {
        while (true)
        {
            anim.SetTrigger("onAttack");
            yield return new WaitForSeconds(attackInterval);
        }
    }

    public void Attack(IBattle target)
    {
        if (target != null)
        {
            float damage = attackPower;
            target.TakeDamage(damage);
        }
    }

    // -------------------------------  HIT  ----------------------------------------
    public void TakeDamage(float damage)
    {
        float finalDamage = damage - defence;

        if (finalDamage < 1)
        {
            finalDamage = 1;
        }
        HP -= finalDamage;

        if (HP > 0)
        {
            anim.SetTrigger("onHit");
            currentSpeed = 0;
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        anim.SetTrigger("onDie");
        status = MonsterCurrentState.DEAD;
        Destroy(gameObject, anim.GetCurrentAnimatorClipInfo(0).Length);
    }

    //########################## Monster Status Check ##################################
    void CheckStatus()
    { // Status Check in Update
        if (!isDead)
        {
            switch (status)
            {
                case MonsterCurrentState.IDLE:
                    Idle();
                    break;

                case MonsterCurrentState.PATROL:
                    Patrol();
                    break;

                case MonsterCurrentState.TRACK:
                    Track();
                    break;

                case MonsterCurrentState.ATTACK:
                    break;

                case MonsterCurrentState.DEAD:
                    //�״� �ִϸ��̼� ���. ��� �Ϸ� �� Monster pool�� ��ȯ
                    break;
            }
        }
    }

    protected void ChangeStatus(MonsterCurrentState newState)
    {
        // On Status Exit
        switch (status)
        {
            case MonsterCurrentState.IDLE:
                break;
            case MonsterCurrentState.PATROL:
                break;
            case MonsterCurrentState.TRACK:
                break;
            case MonsterCurrentState.ATTACK:
                StopCoroutine(attack);
                currentSpeed = moveSpeed;
                break;
            case MonsterCurrentState.DEAD:
                break;
            default:
                break;
        }

        // On Status Enter
        switch (newState)
        {
            case MonsterCurrentState.IDLE:
                trackDirection = Vector2.zero;
                break;
            case MonsterCurrentState.PATROL:
                break;
            case MonsterCurrentState.TRACK:
                currentSpeed = moveSpeed;
                break;
            case MonsterCurrentState.ATTACK:
                StartCoroutine(attack);
                currentSpeed = 0;
                break;
            case MonsterCurrentState.DEAD:
                moveSpeed = 0;
                break;
            default:
                break;
        }
        status = newState;
        anim.SetInteger("CurrentStatus", (int)newState);
    }

    // ########################################### GIZMOS ########################################
    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(this.transform.position, transform.forward, trackDirection.magnitude);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Monster.Enums;

public class Monsters : MonoBehaviour, IHealth, IBattle
{
    private Rigidbody2D rigid = null;
    protected Animator anim = null;
    protected SpriteRenderer sprite = null;

    public int DeadCount = 0;
    
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
    private float currentSpeed = 3.0f;


    // ###################################### VARIABLES #####################################
    // ------------------------------------ TRACK ------------------------------------------
    private float trackTimer = 1.0f;

    // ------------------------------------ TARGET ------------------------------------------
    private Vector2 target = new();

    // ------------------------------------ ATTACK ------------------------------------------
    [SerializeField] protected float attackRange = 1.5f;
    [SerializeField] protected float attackInterval = 3.0f;
    [SerializeField] protected float attackPower = 5.0f;
    [SerializeField] protected float defence = 1.0f;


    private CircleCollider2D cCollider = null;
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

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        cCollider = GetComponent<CircleCollider2D>();

        attack = Attack();

        cCollider.radius = attackRange;
    }

    private void FixedUpdate()
    {
        CheckStatus();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ChangeStatus(MonsterCurrentState.ATTACK);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
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

        waitCounter += Time.fixedDeltaTime;
        if (waitCounter > waitTime)
        {
            waitCounter = 0f;
            waypointIndex++;
            waypointIndex %= waypoint.Length;

            ChangeStatus(MonsterCurrentState.PATROL);
            return;
        }
    }

    // -----------------------------  SEARCH  ------------------------------------------
    bool Search()
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
    void Track()
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

        // waypoint ������ �� ����
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

    void Move_Monster(float speed)
    {
        rigid.position = Vector2.MoveTowards(rigid.position, target, speed * Time.fixedDeltaTime);
        SpriteFlip();
    }

    void SpriteFlip()
    {
        var cross = Vector3.Cross(trackDirection, this.transform.up);
        if (Vector3.Dot(cross, transform.forward) < 0)
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }
    }

    // -------------------------------  ATTACK  ----------------------------------------
    private IEnumerator Attack()
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
                    DeadCount++;
                    //�״� �ִϸ��̼� ���. ��� �Ϸ� �� Monster pool�� ��ȯ
                    break;
            }
        }
    }

    void ChangeStatus(MonsterCurrentState newState)
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
        Handles.DrawWireDisc(this.transform.position, transform.forward, detectRadius);
        Handles.DrawWireDisc(this.transform.position, transform.forward, trackDirection.magnitude);
    }
}

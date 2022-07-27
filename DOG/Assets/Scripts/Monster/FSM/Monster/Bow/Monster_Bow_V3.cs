using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Monster.Enums;

public class Monster_Bow_V3 : MonoBehaviour
{/*
    Arrow arrow;

    private Rigidbody2D rigid = null;
    private SpriteRenderer sprite = null;
    private SpriteRenderer weaponSprite = null;
    private Animator anim = null;
    private Transform weapon = null;

    //################################# Variables ################################
    [Header("몬스터 AI 관련")]
    public MonsterCurrentState status = MonsterCurrentState.IDLE;
    private Vector2 trackDirection = Vector2.zero;
    [SerializeField] private float detectRange = 5.0f;
    private float detectTimer = 0.0f;
    private float detectCoolTime = 1.0f;
    

    public static bool isDead = false;

    [Header("몬스터 기본스탯")]
    [SerializeField] private float healthPoint = 100.0f;
    private float maxHealthPoint = 100.0f;
    [SerializeField] private int strength = 5;
    [SerializeField] private float moveSpeed = 3.0f;

    // ------------------------------------ ATTACK ------------------------------------------
    [SerializeField] protected float attackRange = 1.5f;
    [SerializeField] private float attackCoolTime = 3.0f;
    private float attackTimer = 1.0f;
    [SerializeField] protected float attackPower = 5.0f;
    [SerializeField] protected float defence = 1.0f;

    // ------------------------------------ TARGET ------------------------------------------
    private GameObject target;

    // #################################### Properties #####################################
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

    public Vector2 TrackDirection { get => trackDirection; }

    public System.Action onHealthChange { get; set; }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        CheckStatus();
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

    // -------------------------------  MOVE  ------------------------------------------
    private void Track()
    {
        if (!Search())
        {
            ChangeStatus(MonsterCurrentState.IDLE);
            return;
        }
        else 
        {
            Move_Monster();

            if (InAttackRange())
            {
                Debug.Log("In attack range");
                ChangeStatus(MonsterCurrentState.ATTACK);
                return;
            }
        }
    }

    private bool Search()
    {
        bool result = false;
        Collider2D collider = Physics2D.OverlapCircle(this.transform.position, detectRange, LayerMask.GetMask("Player"));

        if (collider != null)
        {
            target = collider.gameObject;
            result = true;
        }

        return result;
    }

    private bool InAttackRange()
    {
        return (transform.position - target.transform.position).sqrMagnitude < attackRange * attackRange;
    }

    private void Move_Monster()
    {
        trackDirection =  target.transform.position - transform.position;
        anim.SetFloat("Direction_X", trackDirection.x);
        anim.SetFloat("Direction_Y", trackDirection.y);

        rigid.position = Vector2.MoveTowards
            (rigid.position, target.transform.position, moveSpeed * Time.fixedDeltaTime);
    }

    void ShootArrow()
    {
        GameObject arrow = EnemyBulletManager.Inst.GetPooledArrow();
        arrow.transform.position = transform.position;
    }

    void Attack()
    {
        if (InAttackRange())
        {
            attackTimer += Time.fixedDeltaTime;

            if (attackTimer > attackCoolTime)
            {
                Debug.Log("Attack");
                ShootArrow();
                attackTimer = 0.0f;
            }
        }
        else
        {
            detectTimer += Time.fixedDeltaTime;
            if (detectTimer > detectCoolTime)
            {
                ChangeStatus(MonsterCurrentState.TRACK);
                detectTimer = 0f;
                return;
            }
        }
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
                    break;

                case MonsterCurrentState.TRACK:
                    Track();
                    break;

                case MonsterCurrentState.ATTACK:
                    Attack();
                    break;

                case MonsterCurrentState.DEAD:
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
                anim.SetFloat("Speed", moveSpeed);
                break;
            case MonsterCurrentState.ATTACK:
                anim.SetFloat("Speed", 0f);
                attackTimer = 0.5f;
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



    private void OnDrawGizmos()
    {
        Handles.color = Color.green;
        if (status == MonsterCurrentState.ATTACK || status == MonsterCurrentState.TRACK)
        {
            Handles.color = Color.red;
        }

        Handles.DrawWireDisc(transform.position, transform.forward, detectRange);
        Handles.color = Color.white;
        Handles.DrawWireDisc(transform.position, transform.forward, attackRange);
    }*/
}
    
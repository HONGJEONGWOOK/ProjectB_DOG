using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Monster.Enums;

public class Monster_Bow : MonoBehaviour
{/*
    Arrow arrow;

    private Rigidbody2D rigid = null;
    private SpriteRenderer sprite = null;
    public Transform shootPosition = null;
    private Animator anim = null;

    //################################# Variables ################################
    [Header("몬스터 AI 관련")]
    public MonsterCurrentState status = MonsterCurrentState.IDLE;
    private Vector2 trackDirection = Vector2.zero;
    [SerializeField] private float detectRadius = 5.0f;
    private float attackTimer = 1.0f;

    public static bool isDead = false;

    [Header("몬스터 기본스탯")]
    [SerializeField] private float healthPoint = 100.0f;
    private float maxHealthPoint = 100.0f;
    [SerializeField] private int strength = 5;
    [SerializeField] private float moveSpeed = 3.0f;
    private float currentSpeed = 3.0f;

    // ------------------------------------ ATTACK ------------------------------------------
    [SerializeField] protected float attackRange = 1.5f;
    [SerializeField] private float attackInterval = 3.0f;
    [SerializeField] protected float attackPower = 5.0f;
    [SerializeField] protected float defence = 1.0f;
    [SerializeField] private float attackAngle = 45.0f;

    // ------------------------------------ TARGET ------------------------------------------
    protected Vector2 target = new();


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
        if (InAttackRange())
        {
            Debug.Log("in attack range");
            ChangeStatus(MonsterCurrentState.ATTACK);
            return;
        }

        if (Search())
        {
            Move_Monster(currentSpeed);
        }
    }

    private bool Search()
    {
        bool result = false;
        Collider2D collider = Physics2D.OverlapCircle(this.transform.position, detectRadius, LayerMask.GetMask("Player"));

        if (collider)
        {
            Vector2 targetPosition = collider.transform.position;
            if (InSight(targetPosition))
            {
                target = collider.transform.position;
                result = true;
            }
        }

        return result;
    }

    bool InAttackRange()
    {
        bool result = false;

       if ((target - (Vector2)transform.position).sqrMagnitude < attackRange * attackRange)
        {
            result = true;
        }

        return result;
    }

    bool InSight(Vector2 target)
    {
        float angle = Vector2.Angle(transform.right, target - (Vector2)transform.position);
        return angle < (attackAngle * 0.5f);
    }

    private void Move_Monster(float speed)
    {
        trackDirection = target - (Vector2)this.transform.position;
        rigid.position = Vector2.MoveTowards(rigid.position, target, speed * Time.fixedDeltaTime);
        FaceTarget();
    }

    private void FaceTarget()
    {
        transform.right = trackDirection;

        if (trackDirection.x < 0)
        {
            sprite.flipY = true;
        }
        else
        {
            sprite.flipY = false;
        }
    }

    void ShootArrow()
    {
        GameObject arrow = EnemyBulletManager.Inst.GetPooledArrow();
        arrow.transform.position = shootPosition.position;
    }

    void Attack()
    {
        attackTimer -= Time.fixedDeltaTime;

        if (attackTimer < 0)
        {
            anim.SetTrigger("onAttack");
            attackInterval = 1.0f;
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



    private void OnDrawGizmos()
    {
        Vector2 forward = transform.right * detectRadius;
        Quaternion q1 = Quaternion.Euler(0.5f * attackAngle * transform.forward);
        Quaternion q2 = Quaternion.Euler(0.5f * -attackAngle * transform.forward);

        Handles.color = Color.green;
        if (status == MonsterCurrentState.ATTACK || status == MonsterCurrentState.TRACK)
        {
            Handles.color = Color.red;
        }
        
        Handles.DrawLine(transform.position, transform.position + q1 * forward, 2.0f);
        Handles.DrawLine(transform.position, transform.position + q2 * forward, 2.0f);
        Handles.DrawWireArc(transform.position, transform.forward, q2 * forward, attackAngle, detectRadius, 2.0f);
    }*/
}

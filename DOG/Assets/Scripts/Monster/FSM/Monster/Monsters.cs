using System.Collections;
using UnityEngine;
using UnityEditor;

public class Monsters : MonoBehaviour, IHealth, IBattle
{
    Rigidbody2D rigid;
    protected Animator anim;
    protected SpriteRenderer sprite;

    // #################################### VARIABLES #####################################
    private bool isDead = false;

    [Header("Monster AI")]
    [SerializeField] protected MonsterCurrentState status = MonsterCurrentState.IDLE;
    protected Vector2 trackDirection = Vector2.zero;
    [SerializeField] protected float detectRange = 5.0f;

    [Header("Basic Stats")]
    [SerializeField] protected float healthPoint = 100.0f;
    [SerializeField] private float maxHealthPoint = 100.0f;
    [SerializeField] protected int strength = 5;
    [SerializeField] protected float moveSpeed = 3.0f;

    // ------------------------------------ TRACK ------------------------------------------
    protected float currentSpeed = 3.0f;

    // ------------------------------------ TARGET ------------------------------------------
    protected Transform target = null;

    // ------------------------------------ ATTACK ------------------------------------------
    [SerializeField] protected float attackRange = 1.5f;
    [SerializeField] protected float attackCoolTime = 3.0f;
    [SerializeField] private float attackPower = 5.0f;
    [SerializeField] private float defence = 1.0f;
    protected float attackTimer = 1.0f;
    protected float detectTimer = 0.0f;
    [SerializeField] protected float detectCoolTime = 1.0f;
    [Header("Hit")]
    [SerializeField] private float knockbackForce = 0.5f;

    public System.Action onHealthChange { get; set; }

    // ------------------------------------ PATROL ------------------------------------------
    //float waitCounter = 0.0f;
    //float waitTime = 2.0f;
    //int waypointIndex = 0;
    //public Transform[] waypoint = null;


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

    public bool IsDead => isDead;
    public MonsterCurrentState Status => status;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            if (status == MonsterCurrentState.TRACK)
            {
                Track();
            }
        }
    }

    private void Update()
    {
        CheckStatus();
    }

    // ################################### METHODS ##############################################
    // -------------------------------  IDLE  ------------------------------------------
    protected virtual void Idle()
    {
        if (SearchPlayer())
        {
            ChangeStatus(MonsterCurrentState.TRACK);
            return;
        }
    }

    // -----------------------------  SEARCH  ------------------------------------------
    protected bool SearchPlayer()
    {
        bool result = false;

        Collider2D collider = Physics2D.OverlapCircle(this.transform.position, detectRange, LayerMask.GetMask("Player"));

        if (collider)
        {
            target = collider.transform;
            result = true;
        }
        return result;
    }

    // -------------------------------  MOVE  ------------------------------------------
    protected virtual void Track()
    {
        if (!SearchPlayer())
        {
            ChangeStatus(MonsterCurrentState.IDLE);
            return;
        }
        else
        {
            Move_Monster(currentSpeed);
        }
    }

    void Patrol()
    {
        //if (Search())
        //{
        //    ChangeStatus(MonsterCurrentState.TRACK);
        //    return;
        //}

        //if (Vector2.SqrMagnitude(transform.position - waypoint[waypointIndex].position) < 0.01f)
        //{
        //    ChangeStatus(MonsterCurrentState.IDLE);
        //}
        //else
        //{
        //    transform.position = Vector2.MoveTowards(transform.position, waypoint[waypointIndex].position,
        //          Time.fixedDeltaTime * moveSpeed);
        //}
    }

    protected void Move_Monster(float speed)
    {
        trackDirection = (target.position - this.transform.position).normalized;
        rigid.position = Vector2.MoveTowards(
            rigid.position, new Vector2(rigid.position.x, target.position.y), speed * Time.fixedDeltaTime);

        if (IsAtSameHeight())
        {
            rigid.position = Vector2.MoveTowards(rigid.position, target.position, speed * Time.fixedDeltaTime);
            if (InAttackRange())
            {
                ChangeStatus(MonsterCurrentState.ATTACK);
                return;
            }
        }
        SpriteFlip();
    }

    protected virtual bool InAttackRange()
    {
        return (transform.position - target.position).sqrMagnitude < attackRange * attackRange;
    }

    private bool IsAtSameHeight()
    {
        return rigid.position.y - target.position.y < 0.05f;
    }
    protected virtual void SpriteFlip()
    {
        trackDirection = target.position - this.transform.position;
        var cross = Vector3.Cross(trackDirection, this.transform.up);
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
    protected virtual void Attack()
    {
        attackTimer += Time.deltaTime;
        SpriteFlip();

        if (attackTimer > attackCoolTime)
        {
            anim.SetTrigger("onAttack");
            attackTimer = 0.0f;
        }
        else if (!InAttackRange() && attackTimer < attackCoolTime)
        {
            detectTimer += Time.deltaTime;
            if (detectTimer > detectCoolTime)
            {
                ChangeStatus(MonsterCurrentState.TRACK);
                detectTimer = 0f;
                return;
            }
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
            StartCoroutine(KnockBack());
            currentSpeed = 0;
        }
        else
        {
            ChangeStatus(MonsterCurrentState.DEAD);
        }
    }

    protected virtual void Die()
    {
        anim.SetTrigger("onDie");
        currentSpeed = 0;
        StartCoroutine(DisableMonster());
    }

    protected virtual IEnumerator DisableMonster()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length + 2.0f);
        MonsterManager.Inst.ReturnPooledMonster(
            MonsterManager.PooledMonster[MonsterManager.Inst.GoblinID], this.gameObject);
    }

    protected virtual IEnumerator KnockBack()
    { // 따라오는 방향의 반대방향으로 넉백
        float timer = 0f;
        float knockBackTimer = anim.GetCurrentAnimatorClipInfo(0).Length;
        Vector2 knockBackDir = -trackDirection.normalized;
        while (timer < knockBackTimer)
        {
            rigid.position = Vector3.Lerp(rigid.position, knockBackDir, knockbackForce * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
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
                    //Patrol();
                    break;

                case MonsterCurrentState.TRACK:
                    // Fixed Update로 이동
                    break;

                case MonsterCurrentState.ATTACK:
                    Attack();
                    break;

                case MonsterCurrentState.DEAD:
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
                rigid.velocity = Vector2.zero;  //넉백 영향 초기화
                currentSpeed = moveSpeed;
                break;
            case MonsterCurrentState.ATTACK:
                currentSpeed = 0;
                attackTimer = attackCoolTime; // 즉시공격
                break;
            case MonsterCurrentState.DEAD:
                Die();
                break;
            default:
                break;
        }
        status = newState;
        anim.SetInteger("CurrentStatus", (int)newState);
    }

    // ########################################### GIZMOS ########################################
    protected virtual void OnDrawGizmos()
    {
        Handles.color = Color.green;
        if (status == MonsterCurrentState.ATTACK || status == MonsterCurrentState.TRACK)
        {
            Handles.color = Color.red;
        }
        Handles.DrawWireDisc(transform.position, transform.forward, detectRange);
        Handles.color = Color.white;
        Handles.DrawWireDisc(transform.position, transform.forward, attackRange);
    }
}
using System.Collections;
using UnityEngine;
using UnityEditor;
using System.Linq;

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
    private Vector2 knockBackDir = Vector2.zero;
    float knockbackTimer = 0f;
    private float knockBackCoolTime = 0f;
    protected bool isDying = false;

    public System.Action onHealthChange { get; set; }

    // ------------------------------------ PATROL ------------------------------------------
    [Header("Patrol")]
    [SerializeField] float patrolRange = 2.0f;
    float waitCounter = 0.0f;
    [SerializeField] float waitTime = 2.0f;
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


    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    protected virtual void OnEnable()
    {
        foreach (Transform t in waypoint)
        {
            if (transform.localScale.x < 1)
            {
                patrolRange *= 0.8f;
            }
            t.localPosition = Random.insideUnitCircle * patrolRange;
        }
        ChangeStatus(MonsterCurrentState.IDLE);
    }

    private void OnDisable()
    {
        healthPoint = maxHealthPoint;
        rigid.velocity = Vector2.zero;
        foreach (Transform t in waypoint)
        {
            t.localPosition = Vector2.zero ;
        }
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            if (status == MonsterCurrentState.TRACK)
            {
                Track();
            }
            else if (status == MonsterCurrentState.PATROL)
            {
                Patrol();
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
        else
        {
            waitCounter += Time.deltaTime;
            if (waitCounter > waitTime)
            {
                waitCounter = 0;
                ChangeStatus(MonsterCurrentState.PATROL);
            }
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
        }
        else
        {
            Move_Monster(currentSpeed);
        }
    }

    protected virtual void Patrol()
    {
        if (SearchPlayer())
        {
            ChangeStatus(MonsterCurrentState.TRACK);
            return;
        }

        if (( waypoint[waypointIndex].position - transform.position).sqrMagnitude < 0.01f)
        {
            waypointIndex = (waypointIndex + 1) % waypoint.Length;
            ChangeStatus(MonsterCurrentState.IDLE);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, waypoint[waypointIndex].position,
                  Time.fixedDeltaTime * moveSpeed);

            target = waypoint[waypointIndex];
            SpriteFlip();
        }
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

    protected virtual bool InAttackRange() => (transform.position - target.position).sqrMagnitude < attackRange * attackRange;
    private bool IsAtSameHeight() => rigid.position.y - target.position.y < 0.05f;

    protected virtual void SpriteFlip()
    {
        trackDirection = target.position - this.transform.position;
        var cross = Vector3.Cross(trackDirection, this.transform.up);
        sprite.flipX = Vector3.Dot(cross, transform.forward) < 0? true : false;
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

        if (HP > 0 && status != MonsterCurrentState.KNOCKBACK && !isDying)
        {
            ChangeStatus(MonsterCurrentState.KNOCKBACK);
        }
        else if (HP <= 0)
        {
            ChangeStatus(MonsterCurrentState.DEAD);
        }
    }

    protected virtual void Die()
    {
        if (!isDying)
        {
            anim.SetTrigger("onDie");
            currentSpeed = 0;
            StartCoroutine(DisableMonster());
        }
    }

    protected virtual IEnumerator DisableMonster()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length + 2.0f);
        GameObject obj = ItemManager.GetPooledItem(ItemManager.Inst.PooledItems[(int)ItemID.GoblinsPPP]);
        obj.transform.position = transform.position;
        obj.SetActive(true);
        MonsterManager.ReturnPooledMonster(
            MonsterManager.PooledMonster[MonsterManager.Inst.GoblinID], this.gameObject);
    }

    protected void KnockBack()
    {
        //rigid.position = Vector3.Lerp(rigid.position, knockBackDir, knockbackForce * Time.deltaTime);
        knockbackTimer += Time.deltaTime;
        if (knockbackTimer > knockBackCoolTime)
        {
            ChangeStatus(MonsterCurrentState.IDLE);
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

                case MonsterCurrentState.KNOCKBACK:
                    KnockBack();
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
            case MonsterCurrentState.KNOCKBACK:
                rigid.bodyType = RigidbodyType2D.Kinematic;
                knockBackDir = Vector2.zero;
                knockbackTimer = 0f;
                rigid.velocity = Vector2.zero;
                break;
            case MonsterCurrentState.DEAD:
                isDying = false;
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

            case MonsterCurrentState.KNOCKBACK:
                currentSpeed = 0;
                knockbackTimer = 0f;
                anim.SetTrigger("onHit");
                knockBackCoolTime = anim.GetCurrentAnimatorClipInfo(0).Length;
                knockBackDir = transform.position - GameManager.Inst.MainPlayer.transform.position;
                knockBackDir.Normalize();
                rigid.bodyType = RigidbodyType2D.Dynamic;
                rigid.AddForce(knockbackForce * knockBackDir, ForceMode2D.Impulse);
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
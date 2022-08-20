using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Treant : MonoBehaviour, IHealth, IBattle
{
    private Rigidbody2D rigid = null;
    private SpriteRenderer weaponSprite = null;
    public Transform shootPosition = null;
    private Animator anim = null;
    private Animator BowAnim = null;
    private Transform weapon = null;

    //################################# Variables ################################
    [Header("몬스터 AI 관련")]
    public MonsterCurrentState status = MonsterCurrentState.IDLE;
    private Vector2 trackDirection = Vector2.zero;
    [SerializeField] private float detectRange = 5.0f;
    private float detectTimer = 0.0f;
    private float detectCoolTime = 1.0f;

    private bool isDead = false;

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

    [Header("Hit")]
    [SerializeField] private float knockbackForce = 0.5f;

    // ------------------------------------ TARGET ------------------------------------------
    private Transform target;

    // ------------------------------------ PATROL ------------------------------------------
    [Header("Patrol")]
    [SerializeField] float patrolRange = 5.0f;
    float waitCounter = 0.0f;
    [SerializeField] float waitTime = 2.0f;
    int waypointIndex = 0;
    public Transform[] waypoint = null;


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

    public float AttackPower => attackPower;

    public float Defence => defence;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        BowAnim = transform.GetChild(0).GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        weapon = transform.GetChild(0);
        weaponSprite = weapon.GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void OnEnable()
    {
        foreach (Transform t in waypoint)
        {
            t.localPosition = Random.insideUnitCircle * patrolRange;
        }
    }

    private void OnDisable()
    {
        foreach (Transform t in waypoint)
        {
            t.localPosition = Vector2.zero;
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


    //################################## Method ########################################
    // -------------------------------  IDLE  ------------------------------------------
    void Idle()
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

    // -------------------------------  MOVE  ------------------------------------------
    private void Track()
    {
        if (!SearchPlayer())
        {
            ChangeStatus(MonsterCurrentState.IDLE);
            return;
        }

        RotateWeapon();
        Move_Monster();
    }

    private void Patrol()
    {
        if (SearchPlayer())
        {
            ChangeStatus(MonsterCurrentState.TRACK);
            return;
        }

        if (Vector2.SqrMagnitude(transform.position - waypoint[waypointIndex].position) < 0.01f)
        {
            waypointIndex = (waypointIndex + 1) % waypoint.Length;
            ChangeStatus(MonsterCurrentState.IDLE);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, waypoint[waypointIndex].position,
                  Time.fixedDeltaTime * moveSpeed);
        }
    }

    private bool SearchPlayer()
    {
        bool result = false;
        Collider2D collider = Physics2D.OverlapCircle(this.transform.position, detectRange, LayerMask.GetMask("Player"));

        if (collider != null)
        {
            target = collider.transform;
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
        trackDirection = (target.position - this.transform.position).normalized;
        anim.SetFloat("Direction_X", trackDirection.x);
        anim.SetFloat("Direction_Y", trackDirection.y);

        rigid.position = Vector2.MoveTowards(
            rigid.position, new Vector2(rigid.position.x, target.position.y), moveSpeed * Time.fixedDeltaTime);

        if (IsAtSameHeight())
        {
            rigid.position = Vector2.MoveTowards(rigid.position, target.position, moveSpeed * Time.fixedDeltaTime);
            if (InAttackRange())
            {
                ChangeStatus(MonsterCurrentState.ATTACK);
                return;
            }
        }
    }

    private bool IsAtSameHeight()
    {
        return rigid.position.y - target.position.y < 0.05f;
    }

    void ShootArrow()
    {
        GameObject arrow =
            EnemyBulletManager.Inst.GetPooledObject(EnemyBulletManager.PooledObjects[EnemyBulletManager.Inst.ArrowID]);
        arrow.transform.position = shootPosition.position;
        arrow.transform.rotation = weapon.transform.rotation;
        arrow.SetActive(true);
    }

    void Attack()
    {
        if (InAttackRange())
        {
            attackTimer += Time.deltaTime;
            RotateWeapon();

            if (attackTimer > attackCoolTime)
            {
                BowAnim.SetTrigger("onAttack");
                ShootArrow();
                attackTimer = 0.0f;
            }
        }
        else
        {
            detectTimer += Time.deltaTime;
            if (detectTimer > detectCoolTime)
            {
                ChangeStatus(MonsterCurrentState.TRACK);
                detectTimer = 0f;
                return;
            }
            return;
        }
    }
    public void Attack(IBattle target) { }// intentionally Blank

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
        }
        else
        {
            ChangeStatus(MonsterCurrentState.DEAD);
        }
    }
    private void RotateWeapon()
    {
        trackDirection = target.transform.position - transform.position;
        weapon.transform.right = trackDirection;

        anim.SetFloat("Direction_X", trackDirection.x);
        anim.SetFloat("Direction_Y", trackDirection.y);

        if (trackDirection.x < 0)
        {
            weaponSprite.flipY = true;
        }
        else
        {
            weaponSprite.flipY = false;
        }
    }

    private IEnumerator KnockBack()
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
        yield return null;
    }

    //########################## Monster Status Check ##################################
    private void CheckStatus()
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
                break;
            case MonsterCurrentState.ATTACK:
                attackTimer = 0.5f;
                break;
            case MonsterCurrentState.DEAD:
                moveSpeed = 0;
                anim.SetTrigger("onDie");
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
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Treant : MonoBehaviour, IHealth, IBattle
{
    private Rigidbody2D rigid = null;
    private SpriteRenderer weaponSprite = null;
    public Transform shootPosition = null;
    private Animator anim = null;
    private Animator BowAnim = null;
    private Transform weapon = null;
    private AudioSource audioSource;

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
    [SerializeField] private float moveSpeed = 3.0f;

    // ------------------------------------ ATTACK ------------------------------------------
    [SerializeField] protected float attackRange = 1.5f;
    [SerializeField] private float attackCoolTime = 3.0f;
    private float attackTimer = 1.0f;
    [SerializeField] protected float attackPower = 5.0f;
    [SerializeField] protected float defence = 1.0f;
    private bool isDying = false;

    [Header("Hit")]
    [SerializeField] private float knockbackForce = 0.5f;
    private Vector2 knockBackDir = Vector2.zero;
    private float knockbackTimer = 0f;
    private float knockBackCoolTime = 0f;

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
        audioSource = GetComponent<AudioSource>();
    }

    protected virtual void OnEnable()
    {
        healthPoint = maxHealthPoint;
        foreach (Transform t in waypoint)
        {
            t.localPosition = Random.insideUnitCircle * patrolRange;
        }
        ChangeStatus(MonsterCurrentState.IDLE);
    }

    private void OnDisable()
    {
        rigid.velocity = Vector2.zero;
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

    private bool InAttackRange() => (transform.position - target.transform.position).sqrMagnitude < attackRange * attackRange;

    private void Move_Monster()
    {
        trackDirection = (target.position - this.transform.position).normalized;
        anim.SetFloat("Direction_X", trackDirection.x);
        anim.SetFloat("Direction_Y", trackDirection.y);

        rigid.position = Vector2.MoveTowards(
            rigid.position, target.position, moveSpeed * Time.fixedDeltaTime);

        if (InAttackRange())
        {
            ChangeStatus(MonsterCurrentState.ATTACK);
            return;
        }
    }

    void ShootArrow()
    {
        GameObject arrow =
            EnemyBulletManager.Inst.GetPooledObject(ProjectileID.Arrows);
        arrow.transform.position = shootPosition.position;
        arrow.transform.rotation = weapon.transform.rotation;
        if (transform.localScale.x < 1f)
        {
            arrow.transform.localScale = new Vector2(0.5f, 0.5f);
        }
        arrow.SetActive(true);
        audioSource.clip = SoundManager.Inst.clips[(byte)SoundID.ShootArrow].clip;
        audioSource.PlayOneShot(SoundManager.Inst.clips[(byte)SoundID.ShootArrow].clip, 0.3f);
    }

    public void PlayGetHitSound()
    {
        audioSource.PlayOneShot(SoundManager.Inst.clips[(byte)SoundID.TreantGetHit].clip, 0.3f);
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

        if (HP > 0 && status != MonsterCurrentState.KNOCKBACK && !isDying )
        {
            ChangeStatus(MonsterCurrentState.KNOCKBACK);
        }
        else if(HP <= 0)
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

        weaponSprite.flipY = trackDirection.x < 0;
    }

    private void KnockBack()
    { // 따라오는 방향의 반대방향으로 넉백
        knockbackTimer += Time.deltaTime;
        //rigid.position = Vector3.Lerp(rigid.position, knockBackDir, knockbackForce * Time.deltaTime);
        if (knockbackTimer > knockBackCoolTime)
        {
            ChangeStatus(MonsterCurrentState.IDLE);
        }
    }

    private void Die()
    {
        if (!isDying)
        {
            anim.SetTrigger("onDie");
            audioSource.PlayOneShot(SoundManager.Inst.clips[(byte)SoundID.TreantDie].clip, 0.3f);
            StartCoroutine(DisableMonster());
        }
    }

    private IEnumerator DisableMonster()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length + 2.0f);
        GameObject obj = ItemManager.Inst.GetPooledItem(ItemID.Arrows);
        obj.transform.position = transform.position;
        obj.SetActive(true);
        MonsterManager.ReturnPooledMonster(MonsterID.TREANT, this.gameObject);
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
                break;
            case MonsterCurrentState.ATTACK:
                attackTimer = 0.5f;
                break;
            case MonsterCurrentState.KNOCKBACK:
                anim.SetTrigger("onHit");
                knockbackTimer = 0f;
                knockBackCoolTime = anim.GetCurrentAnimatorClipInfo(0).Length;
                knockBackDir =  transform.position - GameManager.Inst.MainPlayer.transform.position;
                knockBackDir = knockBackDir.normalized;

                rigid.bodyType = RigidbodyType2D.Dynamic;
                rigid.AddForce(knockBackDir * knockbackForce, ForceMode2D.Impulse);
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


#if UNITY_EDITOR
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
#endif
}
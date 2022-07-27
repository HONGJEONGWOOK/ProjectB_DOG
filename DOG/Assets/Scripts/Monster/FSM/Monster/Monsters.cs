using UnityEngine;
using UnityEditor;
using Monster.Enums;

public class Monsters : MonoBehaviour
{ 



public static bool isDead = false;
private bool isDead = false;

public System.Action onHit = null;

[Header("���� AI ����")]
public MonsterCurrentState status = MonsterCurrentState.IDLE;
protected Vector2 trackDirection = Vector2.zero;
[SerializeField] protected float detectRadius = 5.0f;
[SerializeField] protected float detectRange = 5.0f;

[Header("���� �⺻����")]
[SerializeField] protected float healthPoint = 100.0f;
[SerializeField] protected int strength = 5;
[SerializeField] protected float moveSpeed = 3.0f;

    // #################################### VARIABLES #####################################
    // ------------------------------------ TRACK ------------------------------------------

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

    protected virtual void Awake()
>>>>>>> Stashed changes:DOG/Assets/Scripts/Monster/FSM/Monster/Monsters.cs
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
    Debug.Log(status);
}
    // -----------------------------  SEARCH  ------------------------------------------
    private bool SearchPlayer()
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
    private void Track()
    {
        if (!SearchPlayer())
        {
            ChangeStatus(MonsterCurrentState.IDLE);
>>>>>>> Stashed changes:DOG/Assets/Scripts/Monster/FSM/Monster/Monsters.cs
            return;
        }
    }

    void Patrol()
{
<<<<<<< Updated upstream:DOG/Assets/Scripts/FSM/Monster/Monsters.cs
    if (Search())
    {
        ChangeStatus(MonsterCurrentState.TRACK);
@ -147,6 + 236,40 @@ public class Monsters : MonoBehaviour
        }

        return result;
=======
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
        trackDirection = target.position - this.transform.position;
        rigid.position = Vector2.MoveTowards(rigid.position, new Vector2(rigid.position.x, target.position.y), speed * Time.fixedDeltaTime);
        //MovePosition(rigid.position + new Vector2(rigid.position.x, target.position.y * speed * Time.fixedDeltaTime));
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
>>>>>>> Stashed changes:DOG/Assets/Scripts/Monster/FSM/Monster/Monsters.cs
    }

    void Move_Monster()
@ -169,12 + 292,45 @@ public class Monsters : MonoBehaviour
        }
    }

<<<<<<< Updated upstream:DOG/Assets/Scripts/FSM/Monster/Monsters.cs
    void Track()
{
    if (!Search())
    {
        ChangeStatus(MonsterCurrentState.IDLE);
        return;
=======
    protected virtual bool InAttackRange()
    {
        return (transform.position - target.position).sqrMagnitude < attackRange * attackRange;
    }
    protected virtual bool IsAtSameHeight()
    {
        return rigid.position.y - target.position.y < 0.05f;
    }

    // -------------------------------  ATTACK  ----------------------------------------
    protected virtual void Attack()
    {
        attackTimer += Time.fixedDeltaTime;

        if (attackTimer > attackCoolTime)
        {
            anim.SetTrigger("onAttack");
            attackTimer = 0.0f;
            return;
        }

        if (!InAttackRange())
        {
            detectTimer += Time.fixedDeltaTime;
            if (detectTimer > detectCoolTime)
            {
                ChangeStatus(MonsterCurrentState.TRACK);
                detectTimer = 0f;
                return;
            }
>>>>>>> Stashed changes:DOG/Assets/Scripts/Monster/FSM/Monster/Monsters.cs
    }

    Move_Monster();
@ -203,7 + 359,41 @@ public class Monsters : MonoBehaviour
        anim.SetTrigger("onHit");
    }

<<<<<<< Updated upstream:DOG/Assets/Scripts/FSM/Monster/Monsters.cs
    void ChangeStatus(MonsterCurrentState newState)
=======
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
                    //Track();
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

    protected void ChangeStatus(MonsterCurrentState newState)
>>>>>>> Stashed changes:DOG/Assets/Scripts/Monster/FSM/Monster/Monsters.cs
{
    // On Status Exit
    switch (status)
@ -215,9 + 405,13 @@ public class Monsters : MonoBehaviour
            case MonsterCurrentState.TRACK:
                break;
            case MonsterCurrentState.ATTACK:
<<<<<<< Updated upstream:DOG/Assets/Scripts/FSM/Monster/Monsters.cs
                StopCoroutine(attack);
break;
            case MonsterCurrentState.HIT:
=======
                currentSpeed = moveSpeed;
>>>>>>> Stashed changes:DOG/Assets/Scripts/Monster/FSM/Monster/Monsters.cs
                break;
            case MonsterCurrentState.DEAD:
                break;
@ -234,11 + 428,17 @@ public class Monsters : MonoBehaviour
            case MonsterCurrentState.PATROL:
                break;
            case MonsterCurrentState.TRACK:
<<<<<<< Updated upstream:DOG/Assets/Scripts/FSM/Monster/Monsters.cs
                break;
            case MonsterCurrentState.ATTACK:
                StartCoroutine(attack);
break;
            case MonsterCurrentState.HIT:
=======
                //currentSpeed = moveSpeed;
                break;
            case MonsterCurrentState.ATTACK:
>>>>>>> Stashed changes:DOG/Assets/Scripts/Monster/FSM/Monster/Monsters.cs
                break;
            case MonsterCurrentState.DEAD:
                break;
@ -249,10 + 449,25 @@ public class Monsters : MonoBehaviour
        anim.SetInteger("CurrentStatus", (int)newState);
    }

<<<<<<< Updated upstream:DOG/Assets/Scripts/FSM/Monster/Monsters.cs

    private void OnDrawGizmos()
{
    Handles.DrawWireDisc(this.transform.position, transform.forward, detectRadius);
    Handles.DrawWireDisc(this.transform.position, transform.forward, trackDirection.magnitude);
=======
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
>>>>>>> Stashed changes:DOG/Assets/Scripts/Monster/FSM/Monster/Monsters.cs
}
}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monsters : MonoBehaviour
{
    private Rigidbody2D rigid = null;
    private Animator anim = null;
    private CircleCollider2D circleColl = null;
    protected SpriteRenderer sprite = null;

    public Transform player = null;
    public enum CurrentState { idle, patrol, track, attack, dead };
    
    public static bool isDead = false;
    private bool isPatrol = false;

    [Header("몬스터 기본스탯")]
    [SerializeField] protected float healthPoint = 100.0f;
    [SerializeField] protected int strength = 5;
    [SerializeField] protected float moveSpeed = 3.0f;
    [SerializeField] protected float attackRange = 1.5f;
    [SerializeField] protected float attackSpeed = 2.0f;

    [Header("몬스터 AI 관련")]
    public CurrentState status = CurrentState.idle;
    private Vector2 trackDirection = Vector2.zero;
    [SerializeField] protected float detectRadius = 5.0f;

    public Transform[] waypoint = null;

    public float HealthPoint { get => healthPoint; set { healthPoint = value; } }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        circleColl = GetComponent<CircleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();

        circleColl.radius = detectRadius;
    }

    private void Start()
    {
        StartCoroutine(CheckStatus());
    }

    private void FixedUpdate()
    {
        Move_Monster();
        //if (isPatrol)
        //{
        //    Patrol();
        //}
    }

    void Move_Monster()
    {
        rigid.MovePosition(rigid.position + moveSpeed * Time.fixedDeltaTime * trackDirection.normalized);
    }

    IEnumerator CheckStatus()
    {
        while (!isDead)
        {
            if (status == CurrentState.idle)
            {
                anim.SetBool("isMoving", false);
                trackDirection = Vector2.zero;
            }
            else if (status == CurrentState.patrol)
            {
                isPatrol = true;
            }
            else if (status == CurrentState.track)
            {
                Track();
            }
            else if (status == CurrentState.attack)
            {
                Attack();
                yield return new WaitForSeconds(attackSpeed);
            }
            else if (status == CurrentState.dead)
            {
                //죽는 애니메이션 재생. 재생 완료 후 Destroy.
                Destroy(this.gameObject);
            }
            yield return null;
        }
    }

    //void Patrol()
    //{
    //    bool isWait = true;
    //    float waitCounter = 0.0f;
    //    float waitTime = 2.0f;

    //    waitCounter += Time.fixedDeltaTime;

    //    if (waitCounter > waitTime)
    //    {
    //        isWait = false;
    //    }

    //    int waypointIndex = 0;
    //    if (Vector2.SqrMagnitude(transform.position - waypoint[waypointIndex].position) < 0.01f)
    //    {
    //        //waypoint 변경
    //        waitCounter = 0.0f;
    //        isWait = true;

    //        waypointIndex = (waypointIndex + 1) % waypoint.Length;
    //    }
    //    else
    //    {
    //        transform.position = Vector2.MoveTowards(transform.position, waypoint[waypointIndex].position,
    //            Time.fixedDeltaTime * moveSpeed);
    //        transform.LookAt(waypoint[waypointIndex]);
    //    }
    //}

    protected virtual void Track()
    {
        anim.SetBool("isMoving", true);
        trackDirection = player.position - this.transform.position;

        // spirte 방향
        if (Vector3.Cross(player.position, this.transform.position).sqrMagnitude < 0) //???
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }

        // 공격사거리 관련
        if (trackDirection.magnitude < attackRange)
        {
            status = CurrentState.attack;
            trackDirection = Vector2.zero;
        }
        else
        {
            status = CurrentState.track;
            anim.SetBool("isAttack", false);
        }
    }

    protected virtual void Attack()
    {
        anim.SetBool("isMoving", false);
        anim.SetBool("isAttack", true);
    }

    private void OnTriggerEnter2D(Collider2D detection)
    {
        if (detection.gameObject.CompareTag("Player"))
        {
            status = CurrentState.track;
        }
    }
    private void OnTriggerExit2D(Collider2D detection)
    {
        status = CurrentState.idle;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
        //Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.3f);
    }
}

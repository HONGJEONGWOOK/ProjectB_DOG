using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monsters : MonoBehaviour
{
    private Rigidbody2D rigid = null;
    protected Animator anim = null;
    private CircleCollider2D circleColl = null;
    protected SpriteRenderer sprite = null;

    public Transform player = null;
    public enum CurrentState { idle, patrol, track, attack, hit, dead };
    
    public static bool isDead = false;

    public System.Action onHit = null;

    [Header("몬스터 기본스탯")]
    [SerializeField] protected float healthPoint = 100.0f;
    [SerializeField] protected int strength = 5;
    [SerializeField] protected float moveSpeed = 3.0f;
    [SerializeField] protected float attackRange = 1.5f;
    [SerializeField] protected float attackDelay = 5.0f;

    [Header("몬스터 AI 관련")]
    public CurrentState status = CurrentState.idle;
    private Vector2 trackDirection = Vector2.zero;
    private bool isAttack = false;
    private IEnumerator attack = null;
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

        attack = Attack();
    }

    private void Start()
    {
        //StartCoroutine(CheckStatus());
    }

    private void FixedUpdate()
    {
        CheckStatus();
    }

    void Move_Monster()
    {
        rigid.position = Vector2.MoveTowards(rigid.position, player.position, moveSpeed * Time.fixedDeltaTime);
    }

    void CheckStatus()
    {
        if (!isDead)
        {
            if (status == CurrentState.idle)
            {
                anim.SetBool("isMoving", false);
                trackDirection = Vector2.zero;
            }
            else if (status == CurrentState.patrol)
            {
                
            }
            else if (status == CurrentState.track)
            {
                trackDirection = player.position - this.transform.position;
                Track();
                StopCoroutine(attack);
            }
            else if (status == CurrentState.attack)
            {
                if (isAttack)
                {
                    trackDirection = player.position - this.transform.position;
                    StartCoroutine(attack);
                    isAttack = false;
                }
            }
            else if (status == CurrentState.hit)
            {
                OnHit();
            }
            else if (status == CurrentState.dead)
            {
                //죽는 애니메이션 재생. 재생 완료 후 Monster pool로 반환
            }
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
        Move_Monster();

        // sprite 방향
        var cross = Vector3.Cross(trackDirection, this.transform.up);
        if (Vector3.Dot(cross, transform.forward) < 0)
        {
            //Debug.Log("왼쪽이다.");
            sprite.flipX = true;
        }
        else
        {
            //Debug.Log("오른쪽이다.");
            sprite.flipX = false;
        }


        // 공격사거리 관련
        if (trackDirection.magnitude < attackRange && trackDirection.y < 0.1f)
        {
            status = CurrentState.attack;
            isAttack = true;
            trackDirection = Vector2.zero;
        }
    }

    protected virtual IEnumerator Attack()
    {
        while (status == CurrentState.attack)
        {
            if (trackDirection.magnitude < attackRange)
            {
                anim.SetBool("isMoving", false);
                anim.SetTrigger("onAttack");
            }
            else if (trackDirection.magnitude > attackRange)
            {
                status = CurrentState.track;
            }
            yield return new WaitForSeconds(attackDelay);
        }
    }

    private void OnHit()
    {
        //anim.SetBool("isHit", true);
        anim.SetTrigger("onHit");
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

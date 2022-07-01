using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monsters : MonoBehaviour
{
    private Rigidbody2D rigid = null;
    private Animator anim = null;
    private CircleCollider2D circleColl = null;

    public Transform player = null;
    public enum CurrentState { idle, patrol, track, attack, dead };
    
    public static bool isDead = false;
    private bool isPatrol = false;

    [Header("몬스터 기본스탯")]
    public float healthPoint = 100.0f;
    public int strength = 5;
    public float moveSpeed = 3.0f;
    public float attackRange = 1.5f;
    public float attackSpeed = 2.0f;

    [Header("몬스터 AI 관련")]
    public CurrentState status = CurrentState.idle;
    private Vector2 trackDirection = Vector2.zero;
    public float detectRadius = 5.0f;

    public Transform[] waypoint = null;

    public float HealthPoint { get => healthPoint; set { healthPoint = value; } }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        circleColl = GetComponent<CircleCollider2D>();

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
                anim.SetBool("isMoving", true);
                trackDirection = player.position - this.transform.position;
                if(trackDirection.sqrMagnitude < attackRange * attackRange)
                {
                    status = CurrentState.attack;
                    trackDirection = Vector2.zero;
                }
            }
            else if (status == CurrentState.attack)
            {
                anim.SetBool("isMoving", false);

                transform.rotation = Quaternion.Euler( transform.position - player.position);
                anim.SetTrigger("OnAttack");
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

    void Patrol()
    {
        bool isWait = true;
        float waitCounter = 0.0f;
        float waitTime = 2.0f;

        waitCounter += Time.fixedDeltaTime;

        if (waitCounter > waitTime)
        {
            isWait = false;
        }

        int waypointIndex = 0;
        if (Vector2.SqrMagnitude(transform.position - waypoint[waypointIndex].position) < 0.01f)
        {
            //waypoint 변경
            waitCounter = 0.0f;
            isWait = true;

            waypointIndex = (waypointIndex + 1) % waypoint.Length;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, waypoint[waypointIndex].position,
                Time.fixedDeltaTime * moveSpeed);
            transform.LookAt(waypoint[waypointIndex]);
        }
    }


    private void OnTriggerEnter2D(Collider2D detection)
    {
        status = CurrentState.track;
    }
    private void OnTriggerExit2D(Collider2D detection)
    {
        status = CurrentState.idle;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange * attackRange);
        //Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.3f);
    }
}

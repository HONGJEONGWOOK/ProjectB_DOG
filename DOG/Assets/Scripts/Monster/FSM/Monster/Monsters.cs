using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Monster.Enums;

public class Monsters : MonoBehaviour
{
    private Rigidbody2D rigid = null;
    protected Animator anim = null;
    protected SpriteRenderer sprite = null;

    public int DeadCount = 0;
    
    public static bool isDead = false;

    public System.Action onHit = null;

    [Header("몬스터 AI 관련")]
    public MonsterCurrentState status = MonsterCurrentState.IDLE;
    protected Vector2 trackDirection = Vector2.zero;
    [SerializeField] protected float detectRadius = 5.0f;

    [Header("몬스터 기본스탯")]
    [SerializeField] protected float healthPoint = 100.0f;
    [SerializeField] protected int strength = 5;
    [SerializeField] protected float moveSpeed = 3.0f;

    //###################################### TRACK ##############################################################
    private Vector2 target = new();

    //###################################### ATTACK #########################################################
    [SerializeField] protected float attackRange = 1.5f;
    [SerializeField] protected float attackDelay = 3.0f;

    private CircleCollider2D cCollider = null;
    IEnumerator attack = null;

    //###################################### PATROL #########################################################
    float waitCounter = 0.0f;
    float waitTime = 2.0f;
    int waypointIndex = 0;
    public Transform[] waypoint = null;

    public float HealthPoint { get => healthPoint; set { healthPoint = value; } }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        cCollider = GetComponent<CircleCollider2D>();

        attack = Attack();

        cCollider.radius = attackRange;
    }

    private void FixedUpdate()
    {
        CheckStatus();
    }

    void CheckStatus()
    {
        if (!isDead)
        {
            //trackDirection = player.position - this.transform.position;
            switch (status)
            {
                case MonsterCurrentState.IDLE:
                    Idle();
                    break;

                case MonsterCurrentState.PATROL:
                    Patrol();
                    break;

                case MonsterCurrentState.TRACK:
                    Track();
                    break;

                case MonsterCurrentState.ATTACK:
                    break;

                case MonsterCurrentState.HIT:
                    OnHit();
                    break;

                case MonsterCurrentState.DEAD:
                    DeadCount++;
                    //죽는 애니메이션 재생. 재생 완료 후 Monster pool로 반환
                    break;
            }
        }
    }

    void Idle()
    {
        if (Search())
        {
            ChangeStatus(MonsterCurrentState.TRACK);
            return;
        }

        waitCounter += Time.fixedDeltaTime;
        if (waitCounter > waitTime )
        {
            waitCounter = 0f;
            waypointIndex++;
            waypointIndex %= waypoint.Length;

            ChangeStatus(MonsterCurrentState.PATROL);
            return;
        }
    }

    void Patrol()
    {
        if (Search())
        {
            ChangeStatus(MonsterCurrentState.TRACK);
            return;
        }

        // waypoint 움직임 및 변경
        if (Vector2.SqrMagnitude(transform.position - waypoint[waypointIndex].position) < 0.01f)
        {
            ChangeStatus(MonsterCurrentState.IDLE);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, waypoint[waypointIndex].position,
                  Time.fixedDeltaTime * moveSpeed);
        }
    }

    bool Search()
    {
        bool result = false;
        Collider2D collider = Physics2D.OverlapCircle(this.transform.position, detectRadius, LayerMask.GetMask("Player"));

        if (collider)
        {
            target = collider.transform.position;
            result = true;
        }

        return result;
    }

    void Move_Monster()
    {
        rigid.position = Vector2.MoveTowards(rigid.position, target, moveSpeed * Time.fixedDeltaTime);



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
    }

    void Track()
    {
        if (!Search())
        {
            ChangeStatus(MonsterCurrentState.IDLE);
            return;
        }

        Move_Monster();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Triggered!");
            ChangeStatus(MonsterCurrentState.ATTACK);
        }
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            anim.SetTrigger("onAttack");
            yield return new WaitForSeconds(attackDelay);
        }
    }

    private void OnHit()
    {
        anim.SetTrigger("onHit");
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
                StopCoroutine(attack);
                break;
            case MonsterCurrentState.HIT:
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
                StartCoroutine(attack);
                break;
            case MonsterCurrentState.HIT:
                break;
            case MonsterCurrentState.DEAD:
                break;
            default:
                break;
        }
        status = newState;
        anim.SetInteger("CurrentStatus", (int)newState);
    }


    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(this.transform.position, transform.forward, detectRadius);
        Handles.DrawWireDisc(this.transform.position, transform.forward, trackDirection.magnitude);
    }
}

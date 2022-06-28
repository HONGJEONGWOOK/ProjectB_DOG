using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monsters : MonoBehaviour
{
    private Rigidbody2D rigid = null;
    private Animator anim = null;
    private CircleCollider2D circleColl = null;

    public Transform player = null;
    public enum CurrentState { idle, traverse, track, attack, dead };
    
    public static bool isDead = false;

    [Header("���� �⺻����")]
    public float healthPoint = 100.0f;
    public int strength = 5;
    public float moveSpeed = 3.0f;
    public float attackRange = 1.5f;

    [Header("���� AI ����")]
    public CurrentState status = CurrentState.idle;
    private Vector2 trackDirection = Vector2.zero;
    public float detectRadius = 5.0f;

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
                //idle �ִϸ��̼� ���
                trackDirection = Vector2.zero;
            }
            else if (status == CurrentState.traverse)
            {

            }
            else if (status == CurrentState.track)
            {
                //Move �ִϸ��̼� ���
                trackDirection = player.position - this.transform.position;
                if(trackDirection.sqrMagnitude < attackRange * attackRange)
                {
                    status = CurrentState.attack;
                }
            }
            else if (status == CurrentState.attack)
            {
                anim.SetTrigger("OnAttack");
                status = CurrentState.track;
            }
            else if (status == CurrentState.dead)
            {
                //�״� �ִϸ��̼� ���. ��� �Ϸ� �� Destroy.
                Destroy(this.gameObject);
            }
            yield return null;
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
        Gizmos.DrawWireSphere(transform.position, detectRadius);
        //Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.3f);
    }

}

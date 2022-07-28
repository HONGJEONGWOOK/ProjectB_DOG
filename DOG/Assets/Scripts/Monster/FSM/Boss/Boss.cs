using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Monster.Enums;

public class Boss : Monsters, IHealth, IBattle
{
    private float attackRand = 0.0f;

    [Header("Meteor Stats")]
    [SerializeField] private float longAttack_Range = 4.0f;
    [Range(0,1f)]
    [SerializeField] private float longRangeAttack_Prob = 1f;
    [SerializeField] private int fireballNum = 5;

    [SerializeField] private Vector2 meteorOffset = Vector2.zero;

    protected override void Attack()
    {
        attackTimer += Time.fixedDeltaTime;

        if (attackTimer > attackCoolTime)
        {
            attackRand = Random.value;
            Debug.Log(attackRand);
            if (attackRand > (1 - longRangeAttack_Prob))
            { // 원거리 공격 메테오
                SpawnMeteor();
            }
            anim.SetFloat("AttackSelector", attackRand);
            anim.SetTrigger("onAttack");
            attackTimer = 0.0f;
        }
        else if ((attackTimer < attackCoolTime) && !InLongRange())
        {
            detectTimer += Time.fixedDeltaTime;
            if (detectTimer > detectCoolTime)
            {
                ChangeStatus(MonsterCurrentState.TRACK);
                detectTimer = 0f;
                return;
            }
        }
    }

    private void SpawnMeteor()
    {
        for (int i = 0; i < fireballNum; i++)
        {
            GameObject ball = EnemyBulletManager.Inst.GetPooledObject(EnemyBulletManager.Inst.PooledObjects["Meteor_Set"]);
            ball.transform.position = Random.insideUnitCircle * transform.position + meteorOffset;
            Debug.Log(Random.insideUnitCircle * transform.position);
        }
    }

    private bool InLongRange() => (transform.position - target.position).sqrMagnitude < longAttack_Range * longAttack_Range;




    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, transform.forward, longAttack_Range);
        if (attackRand > 0.75f)
        {
            Handles.color = Color.red;
        }
    }
}
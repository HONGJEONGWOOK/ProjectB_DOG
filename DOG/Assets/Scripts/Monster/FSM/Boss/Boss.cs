using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Monster.Enums;

public class Boss : Monsters, IHealth, IBattle
{
    private float attackRand = 0.0f;

    [Header("Meteor Stats")]
    [SerializeField] private float longRangeAttack_Range = 4.0f;
    [SerializeField] private float longRangeAttack_Prob = 0.3f;
    [SerializeField] private int fireballNum = 5;

    protected override void Attack()
    {
        attackTimer += Time.fixedDeltaTime;

        if (attackTimer > attackCoolTime)
        {
            attackRand = Random.value;
            Debug.Log(attackRand);
            if (attackRand < (1 - longRangeAttack_Prob))
            { // 근접공격
                return;
            }
            else
            {// 원거리 공격 메테오
                SpawnMeteor();
            }
            anim.SetFloat("AttackSelector", attackRand);
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
        }
    }

    private void SpawnMeteor()
    {
        for (int i = 0; i < fireballNum; i++)
        {
            GameObject ball = EnemyBulletManager.Inst.GetPooledFireBall();
            ball.transform.position = Random.insideUnitCircle * longRangeAttack_Range;
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, transform.forward, longRangeAttack_Range);
        if (attackRand > 0.75f)
        {
            Handles.color = Color.red;
        }
    }
}
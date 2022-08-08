using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster.Enums;

public class Goblin : Monsters, IHealth, IBattle
{
    private CircleCollider2D cCollider = null;

    

    private void Awake()
    {
        cCollider = GetComponent<CircleCollider2D>();
        cCollider.radius = attackRange;

    }

    protected override void Die()
    {
        QuestManager.Instance.GoblinDeadCount++;
        anim.SetTrigger("onDie");
        status = MonsterCurrentState.DEAD;
        Destroy(gameObject, anim.GetCurrentAnimatorClipInfo(0).Length);
    }
}

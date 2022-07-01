using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Bow : Monsters
{
    public static int arrowDirection = 0;

    public Vector2 arrowOffset = Vector2.zero;

    protected override void Attack()
    {
        base.Attack();
        ShootArrow();
    }

    void ShootArrow()
    {
        GameObject arrow = ArrowManager.Arrow_Instance.GetPooledArrow();
        arrow.transform.position = (Vector2)this.transform.position + arrowOffset;
        if (sprite.flipX)
        {
            arrowDirection = -1;
        }
        else
        {
            arrowDirection = 1;
        }
    }
}

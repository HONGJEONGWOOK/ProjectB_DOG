using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Bow : Monsters
{
    private int arrowDirection;

    public Vector2 arrowOffset = Vector2.zero;

    public int ArrowDirection { get => arrowDirection; set { arrowDirection = value; } }

    void ShootArrow()
    {
        GameObject arrow = ArrowManager.Arrow_Instance.GetPooledArrow();
        if (sprite.flipX)
        {
            arrowDirection = -1;
        }
        else
        {
            arrowDirection = 1;
        }

        arrow.transform.position = (Vector2)this.transform.position + arrowDirection * arrowOffset;
    }
}

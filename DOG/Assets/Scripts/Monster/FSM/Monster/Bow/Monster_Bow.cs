using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster.Enums;

public class Monster_Bow : Monsters
{
    public Vector2 arrowOffset = Vector2.zero;

    public Transform shootPosition = null;

    void ShootArrow()
    {
        GameObject arrow = ArrowManager.Arrow_Instance.GetPooledArrow();
        arrow.transform.position = shootPosition.position;
    }
}

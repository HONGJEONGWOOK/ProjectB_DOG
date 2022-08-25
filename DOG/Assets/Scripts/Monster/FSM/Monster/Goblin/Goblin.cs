using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Monsters, IHealth, IBattle
{
    private Transform hitBox;

    protected override void Awake()
    {
        base.Awake();


        hitBox = transform.GetChild(1);
    }

    protected override void SpriteFlip()
    {
        trackDirection = target.position - this.transform.position;
        var cross = Vector3.Cross(trackDirection, this.transform.up);
        if (Vector3.Dot(cross, transform.forward) < 0)
        {   // 왼쪽
            sprite.flipX = true;
            hitBox.localPosition = new Vector3(-1.79f, 0f);
        }
        else
        {   // 오른쪽
            sprite.flipX = false;
            hitBox.localPosition = new Vector3(1.79f, 0f);
        }
    }
}

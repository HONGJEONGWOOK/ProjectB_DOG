using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Monsters, IHealth, IBattle
{
    private GameObject hpUI = null;
    private Transform hitBox;


    

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

    protected override void Awake()
    {
        base.Awake();

        hpUI = transform.GetComponentInChildren<HP_Bar_Monster>().gameObject;
        hpUI.SetActive(false);

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

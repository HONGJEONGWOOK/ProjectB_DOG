using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackTrigger : MonoBehaviour
{
    float power;

    private void Awake()
    {
        power = transform.parent.GetComponent<Boss>().AttackPower;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            IBattle target = collision.GetComponent<IBattle>();
            if (target != null)
            {
                target.TakeDamage(power);
            }
        }
    }
}

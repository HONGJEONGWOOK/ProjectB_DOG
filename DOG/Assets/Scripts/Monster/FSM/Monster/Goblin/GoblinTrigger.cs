using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinTrigger : MonoBehaviour
{
    float AttackPower;

    private void Awake()
    {
        AttackPower = transform.parent.GetComponent<Goblin>().AttackPower;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            IBattle target = collision.GetComponent<IBattle>();
            target.TakeDamage(AttackPower) ;
        }
    }
}

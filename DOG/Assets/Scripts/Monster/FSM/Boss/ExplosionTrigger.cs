using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTrigger : MonoBehaviour
{
    float power;

    private void Awake()
    {
        power = transform.parent.GetComponent<Meteor>().Power;
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

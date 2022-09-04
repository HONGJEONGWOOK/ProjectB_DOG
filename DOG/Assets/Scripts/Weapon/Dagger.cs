using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            IBattle target = collision.GetComponent<IBattle>();
            target.TakeDamage(GameManager.Inst.MainPlayer.AttackPower);
        }
    }
}

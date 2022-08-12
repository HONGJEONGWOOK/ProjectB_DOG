using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//검
public class Sword : MonoBehaviour
{
    

    private void OnCollisionEnter2D(Collision2D collision)
    {


        if (collision.collider.CompareTag("Monster"))
        {
            IBattle target = collision.GetComponent<IBattle>();
            target.TakeDamage(AttackPower);
        }
    }

}

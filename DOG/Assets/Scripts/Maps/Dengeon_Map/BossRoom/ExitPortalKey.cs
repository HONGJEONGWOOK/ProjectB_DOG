using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class ExitPortalKey : MonoBehaviour
{
    public System.Action onKeyLoot;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onKeyLoot.Invoke();
            Destroy(this.gameObject);
        }
    }
}

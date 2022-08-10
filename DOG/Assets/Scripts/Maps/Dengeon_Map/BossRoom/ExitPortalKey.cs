using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class ExitPortalKey : MonoBehaviour
{
    ExitPortal portal;

    private void Awake()
    {
        portal = FindObjectOfType<ExitPortal>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            portal.ShowPortal();
            Destroy(this.gameObject);
        }
    }
}

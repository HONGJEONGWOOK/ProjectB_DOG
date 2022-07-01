using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Arrow : MonoBehaviour
{
    private Rigidbody2D rigid = null;

    private Vector2 flyDirection = Vector2.zero;

    public float shootSpeed = 3.0f;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        flyDirection *= Monster_Bow.arrowDirection;
        rigid.velocity = shootSpeed * transform.right; //* Monster_Bow.arrowDirection;
    }

    private void OnDisable()
    {
        flyDirection = Vector2.zero;
        rigid.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Border") || collision.gameObject.CompareTag("Player"))
        {
            ArrowManager.Arrow_Instance.ReturnPooledArrow(this.gameObject);
        }
    }
}

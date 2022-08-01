using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ArrowV2 : MonoBehaviour
{
    private Rigidbody2D rigid = null;

    public float shootSpeed = 3.0f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rigid.velocity = shootSpeed * transform.right;
    }

    private void OnDisable()
    {
        //Monster_Bow.arrowDirection = 1;
        rigid.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EnemyBulletManager.Inst.ReturnPooledEnemy(EnemyBulletManager.PooledObjects[EnemyBulletManager.Inst.ArrowID],
                                                  this.gameObject);
        }
    }
}

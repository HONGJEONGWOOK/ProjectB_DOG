using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ArrowV2 : MonoBehaviour
{
    private Rigidbody2D rigid = null;

    public float shootSpeed = 3.0f;
    private float damage;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        damage = 5;//MonsterManager.Inst.PoolingMonsters[(int)MonsterID.TREANT].prefab.GetComponent<Treant>().AttackPower;
    }

    private void Start()
    {
        rigid.velocity = shootSpeed * transform.right;
    }

    private void OnDisable()
    {
        rigid.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IBattle target = collision.GetComponent<IBattle>();
            target.TakeDamage(damage);
        }
        EnemyBulletManager.Inst.ReturnPooledEnemy(EnemyBulletManager.PooledObjects[EnemyBulletManager.Inst.ArrowID],
                                                  this.gameObject);
    }
}

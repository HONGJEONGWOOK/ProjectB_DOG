using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ArrowV2 : MonoBehaviour
{
    private Rigidbody2D rigid = null;

    [SerializeField] float shootSpeed = 5.0f;

    float playerAttackPower;
    float treantAttackPower;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        playerAttackPower = GameManager.Inst.MainPlayer.AttackPower;
        Treant treaTemp = FindObjectOfType<Treant>();
        if (treaTemp != null)
        {
            treantAttackPower = treaTemp.AttackPower;
        }

        rigid.velocity = shootSpeed * transform.right;
    }

    private void OnDisable()
    {
        rigid.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IBattle target = collision.GetComponent<IBattle>();
        if (target != null)
        {
            float damage = 1f ;
            if (collision.CompareTag("Player"))
            {
                damage = treantAttackPower;
            }
            else if (collision.CompareTag("Monster"))
            {
                damage = playerAttackPower;
            }

            target.TakeDamage(damage);
        }
        EnemyBulletManager.Inst.ReturnPooledObject(ProjectileID.Arrows, this.gameObject);
    }
}

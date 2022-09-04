using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Arrow : MonoBehaviour
{
    public float lifeTime = 3.0f;   // 총알의 수명
    public float speed = 10.0f;
    Rigidbody2D rigid = null;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        Destroy(this.gameObject, lifeTime);
    }

    private void Start()
    {
        rigid.velocity = transform.right * speed;
        Destroy(this.gameObject, lifeTime);     // lifeTime초 후에 게임 오브젝트를 삭제한다.
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            IBattle target = collision.GetComponent<IBattle>();
            target.TakeDamage(GameManager.Inst.MainPlayer.AttackPower);
        }
        Destroy(this.gameObject);
    }
}

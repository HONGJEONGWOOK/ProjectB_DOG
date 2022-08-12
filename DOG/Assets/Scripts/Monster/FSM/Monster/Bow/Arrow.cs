using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Arrow : MonoBehaviour
{/*
    private Rigidbody2D rigid = null;
    private SpriteRenderer arrowSprite = null;

    public float shootSpeed = 3.0f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        arrowSprite = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        rigid.velocity = shootSpeed * MonsterManager.Inst.Monster_Bow_V3.TrackDirection.normalized;
        this.transform.right = MonsterManager.Inst.Monster_Bow_V3.TrackDirection;
        Debug.Log(MonsterManager.Inst.Monster_Bow_V3.TrackDirection);
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
            EnemyBulletManager.Inst.ReturnPooledArrow(this.gameObject);
        }
    }*/
}

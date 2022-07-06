using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Arrow : MonoBehaviour
{
    private Rigidbody2D rigid = null;
    private SpriteRenderer arrowSprite = null;
    Monster_Bow monster = null;

    public float shootSpeed = 3.0f;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        arrowSprite = GetComponent<SpriteRenderer>();
        monster = FindObjectOfType<Monster_Bow>();
    }

    private void OnEnable()
    {
        rigid.velocity = shootSpeed * monster.ArrowDirection * transform.right; //* Monster_Bow.arrowDirection;
        
        if(monster.ArrowDirection == 1)
        {
            arrowSprite.flipX = false;
        }
        else if(monster.ArrowDirection == -1)
        {
            arrowSprite.flipX = true;
        }
    }

    private void OnDisable()
    {
        //Monster_Bow.arrowDirection = 1;
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

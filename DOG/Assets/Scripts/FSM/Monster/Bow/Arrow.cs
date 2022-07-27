using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Arrow : MonoBehaviour
{
    private Rigidbody2D rigid = null;

    public float shootSpeed = 3.0f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
<<<<<<< Updated upstream:DOG/Assets/Scripts/FSM/Monster/Bow/Arrow.cs
        rigid.velocity = shootSpeed * ArrowManager.Arrow_Instance.ArrowDirection * transform.right; //* Monster_Bow.arrowDirection;
        
        if(ArrowManager.Arrow_Instance.ArrowDirection == 1)
        {
            arrowSprite.flipX = false;
        }
        else if(ArrowManager.Arrow_Instance.ArrowDirection == -1)
        {
            arrowSprite.flipX = true;
        }
=======
        rigid.velocity = shootSpeed * MonsterManager.Inst.Monster_Bow.TrackDirection.normalized;
        this.transform.right = MonsterManager.Inst.Monster_Bow.TrackDirection;
>>>>>>> Stashed changes:DOG/Assets/Scripts/Monster/FSM/Monster/Bow/Arrow.cs
    }

    private void OnDisable()
    {
        rigid.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
<<<<<<< Updated upstream:DOG/Assets/Scripts/FSM/Monster/Bow/Arrow.cs
        if (collision.gameObject.CompareTag("Border") || collision.gameObject.CompareTag("Player"))
=======
        if (collision.gameObject.CompareTag("Player")||collision.gameObject.CompareTag("Wall"))
>>>>>>> Stashed changes:DOG/Assets/Scripts/Monster/FSM/Monster/Bow/Arrow.cs
        {
            EnemyBulletManager.Inst.ReturnPooledArrow(this.gameObject);
        }
    }
}

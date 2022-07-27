using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameManager manager;
    GameObject scanObject;

    PlayerInputActions actions = null;
    Animator anim = null;

    public float hp = 10.0f;
    Rigidbody2D rigid = null;

    public float moveSpeed = 5.0f;
    public float currentHP = 0.0f;

    private Vector3 direction = Vector3.zero;

    public Slider hpSlider;
    public Slider hpSlider2;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        actions = new PlayerInputActions();
        rigid = GetComponent<Rigidbody2D>();
        
        currentHP = hp;
    }
   
    private void OnEnable()
    {
        actions.Player.Enable();
        actions.Player.Move.performed += OnMoveInput;
        actions.Player.Move.canceled += OnMoveInput;
        actions.Player.Attack.performed += OnAttack;
        actions.Player.Talk.performed += OnTalk;
    }

    private void OnDisable()
    {
        actions.Player.Talk.performed -= OnTalk;
        actions.Player.Attack.performed -= OnAttack;
        actions.Player.Move.canceled -= OnMoveInput;
        actions.Player.Move.performed -= OnMoveInput;
        actions.Player.Disable();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        //HpControlor();
    }

    private void Move()
    {
        rigid.MovePosition(transform.position + (direction * moveSpeed * Time.deltaTime));
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        
        //equipWeapon.Use();
        //Weapon.instance.Swing();
    }

    /* private void HpControlor()
     {
         hpSlider.maxValue = hp;
         hpSlider2.maxValue = hp;

         hpSlider.value = currentHP;
         hpSlider2.value = currentHP;

         if (currentHP > 0.1)
         {
             currentHP -= 0.1f;
         }
     }*/

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();

        if (direction.x != 0 || direction.y != 0)
        {
            anim.SetBool("input", true);

            anim.SetFloat("inputx", direction.x);
            anim.SetFloat("inputy", direction.y);
        }
        else
            anim.SetBool("input", false);
    }

    private void OnTalk(InputAction.CallbackContext _)
    {
        SearchNpc();

        if (scanObject != null)
        {
            manager.AskAction(scanObject);
        }
        else
        {
            Debug.Log("대상이 없습니다.");
        }
    }

    void SearchNpc()
    {
        //int layerNpc = 6 << (LayerMask.NameToLayer("Npc"));

        //Collider2D[] rayHit = Physics2D.OverlapCircle(transform.position, 1.5f, LayerMask.);
        Collider2D[] col = Physics2D.OverlapCircleAll(transform.position, 1.5f, 1 << 6);

        if( col.Length > 0)
        {
            scanObject = col[0].gameObject;
        }
        else
        {
            Debug.Log("대상이 없습니다.");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }
}

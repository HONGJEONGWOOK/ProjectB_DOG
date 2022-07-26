using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player_Hero : MonoBehaviour
{
    public GameManager manager;
    GameObject scanObject;

    PlayerInputActions actions;
    Animator anim;
    Rigidbody2D rigid = null;
    BoxCollider2D Collider;


    public GameObject shootPrefab = null;
    public float moveSpeed = 5.0f;

    private Vector3 direction = Vector3.zero;

    private void Awake()
    {
        actions = new();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        Collider = GetComponent<BoxCollider2D>();
    }
   

    private void OnEnable()
    {
        actions.Player.Enable();
        actions.Player.Move.performed += OnMove;
        actions.Player.Move.canceled += OnMove;
        actions.Player.Attack.performed += OnAttack;
        actions.Player.Talk.performed += OnTalk;
    }

    

    private void OnDisable()
    {
        actions.Player.Talk.performed -= OnTalk;
        actions.Player.Attack.performed -= OnAttack;
        actions.Player.Move.canceled -= OnMove;
        actions.Player.Move.performed -= OnMove;
        actions.Player.Disable();
    }

    private void FixedUpdate()
    {
        Move();
        SearchNpc();
    }

    private void Move()
    {
        rigid.MovePosition(transform.position + (direction * moveSpeed * Time.fixedDeltaTime));
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();

        if (direction.x != 0 || direction.y != 0)
        {
            anim.SetBool("Input", true);

            anim.SetFloat("X", direction.x);
            anim.SetFloat("Y", direction.y);
        }
        else
            anim.SetBool("Input", false);
    }
    // 움직일때 마지막에 봤던 방향으로 멈춰있기

    private void OnTalk(InputAction.CallbackContext _)
    {
        if (scanObject != null)
        {
            manager.AskAction(scanObject);
        }
        else
        {
            Debug.Log("����� �����ϴ�.");
        }
    }

    void SearchNpc()
    {
        Vector3 dir = direction;

        Debug.DrawRay(rigid.position, dir * 1.5f, Color.red);
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dir, 1.5f, LayerMask.GetMask("Npc"));

        if (rayHit.collider != null)
        {
            scanObject = rayHit.collider.gameObject;
        }
        else
        {
            scanObject = null;
        }
    }

    // 자기가 보고있는 방향으로 공격하기
    private void OnAttack(InputAction.CallbackContext context)
    {
        Debug.Log("공격 및 말걸기");
        if (scanObject != null)
        {
            manager.AskAction(scanObject);
        }

        anim.SetTrigger("Attack");
    }

    



}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    PlayerInputActions actions = null;

    public float hp = 10;
    public float currentHp;


    Animator anim = null;
    Rigidbody2D rigid = null;

    public float moveSpeed = 5.0f;

    private Vector3 direction = Vector3.zero;

    public Slider hpSlider;
    public Slider hpSlider2;


    private void Start()
    {
        currentHp = hp;
    }
    private void Awake()
    {
        actions = new PlayerInputActions();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        actions.Player.Enable();
        actions.Player.Move.performed += OnMoveInput;
        actions.Player.Move.canceled += OnMoveInput;
    }

   

    private void OnDisable()
    {
        actions.Player.Move.canceled -= OnMoveInput;
        actions.Player.Move.performed -= OnMoveInput;
        actions.Player.Disable();
    }


    private void Update()
    {
        hpSlider.maxValue = hp;
        hpSlider.value = currentHp;

        hpSlider2.maxValue = hp;
        hpSlider2.value = currentHp;

        
            
    }
    private void FixedUpdate()
    {
        currentHp -= 0.1f;

        Move();

        
    }

    void Move()
    {
        rigid.MovePosition(transform.position + (direction * moveSpeed * Time.deltaTime));

        if (direction.x != 0 || direction.y != 0)
            anim.SetBool("input", true);

        anim.SetFloat("inputx", direction.x);
        anim.SetFloat("inputy", direction.y);


    }


    private void OnMoveInput(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
        
       

    }

}

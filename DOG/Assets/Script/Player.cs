using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    PlayerInputActions actions = null;

    Animator anim = null;
    Rigidbody2D rigid = null;

    public float moveSpeed = 5.0f;

    private Vector3 direction = Vector3.zero;

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

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        rigid.MovePosition(transform.position + (direction * moveSpeed * Time.deltaTime));

        if (direction.y > 0)
            anim.SetBool("Up", true);
        else if (direction.x != 0)
            anim.SetBool("Side", true);
        else if (direction.y < 0)
            anim.SetBool("Down", true);
        else
        {
            anim.SetBool("Down", false);
            anim.SetBool("Up", false);
            anim.SetBool("Side", false);
        }
    }


    private void OnMoveInput(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
        
       

    }

}

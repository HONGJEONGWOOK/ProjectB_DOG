
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    PlayerInput playerInput = null;
    Rigidbody2D rigid = null;
    //Animator anim = null;

    public float moveSpeed = 5.0f;

    private Vector2 inputDir = Vector2.zero;

    private void Awake()
    {
        playerInput = new PlayerInput();
        rigid = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
    }
    private void OnEnable()
    {
        playerInput.Player.Enable();
        playerInput.Player.Move.performed += OnMoveInput;
        playerInput.Player.Move.canceled += OnMoveInput;
    }

    private void OnDisable()
    {
        playerInput.Player.Move.performed -= OnMoveInput;
        playerInput.Player.Move.canceled -= OnMoveInput;
        playerInput.Player.Disable();
    }

    void Move()
    {
        rigid.MovePosition( (Vector2)transform.position + (moveSpeed * Time.fixedDeltaTime * inputDir));
        
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector2>();
    }

}

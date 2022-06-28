using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Move : MonoBehaviour
{
    Player_Input actions = null;
    Rigidbody2D rigid = null;
    Vector2 inputDir = Vector2.zero;

    public float moveSpeed = 5.0f;

    private void Awake()
    {
        actions = new Player_Input();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        actions.Player.Move.Enable();
        actions.Player.Move.performed += OnMoveInput;
        actions.Player.Move.canceled += OnMoveInput;
    }

    private void OnDisable()
    {
        actions.Player.Move.Disable();
        actions.Player.Move.performed -= OnMoveInput;
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        rigid.MovePosition(rigid.position + (moveSpeed * Time.fixedDeltaTime * inputDir));
    }

    void OnMoveInput(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector2>();
    }
}

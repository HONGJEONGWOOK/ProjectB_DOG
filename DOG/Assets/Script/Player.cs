using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    PlayerInputActions actions = null;

    public float hp = 10.0f;

    Animator anim = null;
    Rigidbody2D rigid = null;

    public float moveSpeed = 5.0f;
    public float currentHP = 0.0f;

    private Vector3 direction = Vector3.zero;

    public Slider hpSlider;
    public Slider hpSlider2;


   
    private void Awake()
    {
       
        actions = new PlayerInputActions();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //equipWeapon = new Weapon();

        currentHP = hp;




    }
   

    private void OnEnable()
    {
        actions.Player.Enable();
        actions.Player.Move.performed += OnMoveInput;
        actions.Player.Move.canceled += OnMoveInput;
        actions.Player.Attack.performed += OnAttack;
    }

   

    private void OnDisable()
    {
        actions.Player.Attack.performed -= OnAttack;
        actions.Player.Move.canceled -= OnMoveInput;
        actions.Player.Move.performed -= OnMoveInput;
        actions.Player.Disable();
    }


    
    private void FixedUpdate()
    {
        hpSlider.maxValue = hp;
        hpSlider2.maxValue = hp;

<<<<<<< Updated upstream:DOG/Assets/Script/Player.cs
        hpSlider.value = currentHP;
        hpSlider2.value = currentHP;
=======
        if (direction.y == 1)
        {
            direction = Vector3.up;
        }
        else if (direction.y == -1)
        {
            direction = Vector3.down;
        }
        else if (direction.x == 1)
        {
            direction = Vector3.right;
            //flip x
        }
        else if (direction.x == -1)
        {
            direction = Vector3.left;
        }
>>>>>>> Stashed changes:DOG/Assets/Scripts/Player/Player.cs

        /*if (currentHP > 0)
        {
            currentHP -= 0.1f;
        }*/
        Move();

<<<<<<< Updated upstream:DOG/Assets/Script/Player.cs
        
=======
        HpControlor();
>>>>>>> Stashed changes:DOG/Assets/Scripts/Player/Player.cs
    }

    private void Move()
    {
        rigid.MovePosition(transform.position + (direction * moveSpeed * Time.deltaTime));

        if (direction.x != 0 || direction.y != 0)
        {
<<<<<<< Updated upstream:DOG/Assets/Script/Player.cs
            anim.SetBool("input", true);

            anim.SetFloat("inputx", direction.x);
            anim.SetFloat("inputy", direction.y);
        }
        else
            anim.SetBool("input", false);
    }

    private void OnAttack(InputAction.CallbackContext context)
=======
            manager.AskAction(scanObject);
        }
        Weapon.instance.Swing();
        //칼이 공격할때만 변경하기
        
    }

    private void HpControlor()
>>>>>>> Stashed changes:DOG/Assets/Scripts/Player/Player.cs
    {
        //equipWeapon.Use();

    }

<<<<<<< Updated upstream:DOG/Assets/Script/Player.cs
=======
        if (currentHP > 0.1)
        {
            currentHP -= 0.1f;
        }
    }
>>>>>>> Stashed changes:DOG/Assets/Scripts/Player/Player.cs

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
        
       

    }

}

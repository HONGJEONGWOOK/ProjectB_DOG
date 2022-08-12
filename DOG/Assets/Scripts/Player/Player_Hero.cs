using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine.UI;



public class Player_Hero : MonoBehaviour, IHealth,IBattle
{

    //IHealth--------------------------------------------------------------------------------
    public float hp = 100.0f;
    float maxHP = 100.0f;


    public float HP
    {
        get => hp;
        set 
        {
            if(hp != value)
            {
                hp = Mathf.Clamp(value, 0, maxHP);
                onHealthChange?.Invoke();
            }
        }
    }

    public float MaxHP
    {
        get => maxHP;
    }

    public System.Action onHealthChange { get; set; }

    //IBattle--------------------------------------------------------------------------------
    public float attackPower = 30.0f;
    public float defencePower = 10.0f;
    public float criticalRate = 0.3f;



    public float AttackPower { get => attackPower; }

    public float Defence { get => defencePower; }


    //--------------------------------------------------------------------------------

    public GameManager manager;
    GameObject scanObject;
    GameObject sword;

    PlayerInputActions actions;
    Animator anim;
    Rigidbody2D rigid = null;
    CapsuleCollider2D Collider;


    public GameObject shootPrefab = null;
    public float moveSpeed = 5.0f;

    private Vector3 direction = Vector3.zero;

    public PlayerInputActions Actions => actions;


    private void Awake()
    {
        actions = new();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        Collider = GetComponent<CapsuleCollider2D>();
    }
   

    private void OnEnable()
    {
        actions.Player.Enable();
        actions.Player.Move.performed += OnMove;
        actions.Player.Move.canceled += OnMove;
        actions.Player.Attack.performed += OnAttack;
        actions.UI.Enable();
        actions.UI.Escape.performed += OnEscape;
    }

    

    private void OnDisable()
    {
        actions.UI.Escape.performed -= OnEscape;
        actions.UI.Disable();
        actions.Player.Attack.performed -= OnAttack;
        actions.Player.Move.canceled -= OnMove;
        actions.Player.Move.performed -= OnMove;
        actions.Player.Disable();
    }

    public void Attack(IBattle target)
    {
        if (target != null)
        {
            float damage = AttackPower;
            if (Random.Range(0.0f, 1.0f) < criticalRate)
            {
                damage *= 2.0f;
            }
            target.TakeDamage(damage);
        }
    }

    public void TakeDamage(float damage)
    {
        float finalDamage = damage - defencePower;
        if (finalDamage < 1.0f)
        {
            finalDamage = 1.0f;
        }

        HP -= finalDamage;


        if (HP > 0.0f)
        {
            Debug.Log($"{hp}");
            //살아있다.
            anim.SetTrigger("Hit");
        }
        else
        {
            Debug.Log("죽음");
        }

    }


    private void FixedUpdate()
    {
        Move();
        SearchNpc();
    }

    // 체력 만들고
    // 맞을때 어떻게 할지 생각해야하고
    // 맞을때 캐릭 잠깐동안 깜빡이고 무적상태
    // 뒤로 밀려남 

    // esc눌렀을때 일시정지 및 옵션 생성
    // 

    private void OnCollisionEnter2D(Collision2D collision)
    {
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
        

        anim.SetTrigger("Attack");

    }



    private void OnEscape(InputAction.CallbackContext obj)
    {
        Debug.Log("메뉴");
    }


}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameManager manager;


    PlayerInputActions actions = null;
    GameObject scanObject;
    Weapon sword = null;
    Animator sword_Animator = null;
    Animator arm_Animator = null;
    Animator bodyAnimtor = null;

    public float hp = 10.0f;

    Rigidbody2D rigid = null;

    public float moveSpeed = 5.0f;
    public float currentHP = 0.0f;

    private Vector3 direction = Vector3.zero;

    public Slider hpSlider;
    public Slider hpSlider2;

    bool isAction;

    //test--------------------------------------------------------------------
    Animator anim;

    private void Awake()
    {
        
        actions = new PlayerInputActions();
        rigid = GetComponent<Rigidbody2D>();

        sword = FindObjectOfType<Weapon>();
        sword_Animator = sword.GetComponent<Animator>();
        //arm_Animator = transform.Find("Player_Arm").GetComponent<Animator>();
        //bodyAnimtor = transform.Find("Player_Body").GetComponent<Animator>();
        anim = GetComponent<Animator>();

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
        Debug.DrawRay(rigid.position, direction * 1.0f, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, direction, 1.0f, LayerMask.GetMask("Npc"));
        //???????????? ????????? ????????? ?????? ????????? ??????????????? direction??? ?????? ?????? ????????? ??? ??????

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
        }
        else if (direction.x == -1)
        {
            direction = Vector3.left;
        }

        if (rayHit.collider != null)
        {
            scanObject = rayHit.collider.gameObject;
        }
        else
        {
            scanObject = null;
        }

    }

    private void Move()
    {
        rigid.MovePosition(transform.position + (direction * moveSpeed * Time.fixedDeltaTime));
    }

    private void OnAttack(InputAction.CallbackContext _)
    {
        //equipWeapon.Use();
        //Weapon.instance.Swing();
        Attack_Sword();
    }

    void Attack_Sword()
    {
        sword_Animator.SetTrigger("OnAttack");
        arm_Animator.SetTrigger("OnAttack");
        bodyAnimtor.SetTrigger("OnAttack");
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
            AskAction(scanObject);

        }
        else
        {
            Debug.Log("????????? ????????????.");
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
            Debug.Log("????????? ????????????.");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }

    //?????? ???????????? ????????? scanObj??????????????? ????????? GameObject???????????? scanObject??? ??????
    //scanObject??? GetComponent?????? ????????? ObjectData????????? objData??? ??????
    //ObjectData????????? public int id; public bool isNpc; ??? ????????? ??????
    //
    public void AskAction(GameObject scanObj)
    {
        scanObject = scanObj;
        ObjectData objData = scanObject.GetComponent<ObjectData>();
        Talk(objData.id, objData.isNpc);

        //????????? ???
        GameManager.Inst.TalkPanel.SetActive(isAction);
    }

    void Talk(int id, bool isNpc)
    {
        isAction = false;
        //QuestManager??? GetQuestTalkIndex??? ??????????????? ????????? questTalkIndex
        //GetQuestTalkIndex??? questId ?????? ??????
        int questTalkIndex = QuestManager.Instance.GetQuestTalkIndex(id);

        //questId??? 
        string talkData =  GameManager.Inst.talkManager.GetTalk(id + questTalkIndex, QuestManager.Instance.TalkIndex);

        if (isNpc)
        {
            GameManager.Inst.talkText.text = talkData;
        }
        else
        {
            GameManager.Inst.talkText.text = talkData;
        }

        //?????????
        if (talkData == null)
        {
            isAction = false;
            QuestManager.Instance.TalkIndex = 0;
            Debug.Log(QuestManager.Instance.CheckQuest(id));
            return;
        }

        isAction = true;
        QuestManager.Instance.TalkIndex++;
    }
}

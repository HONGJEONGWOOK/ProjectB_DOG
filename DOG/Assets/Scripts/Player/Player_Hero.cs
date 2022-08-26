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

    public Puzzle pz;
    public MiniPuzzle mpz;

    bool isAction = false;

    public GameObject shootPrefab = null;
    public float moveSpeed = 5.0f;

    private Vector3 direction = Vector3.zero;

    public PlayerInputActions Actions => actions;


    private void OnCollisionEnter2D(Collision2D col)    // 돌 움직이게하는
    {
        if (col.gameObject.CompareTag("Rock"))
        {
            col.rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        if(col.gameObject.CompareTag("Box"))
        {
           // mpz.Init();
        }

    }
    private void OnCollisionExit2D(Collision2D other)   //돌을 뒤의 돌과 충돌이 되게해서 멈추게
    {
        if (other.gameObject.CompareTag("Rock"))
        {
            other.rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

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
        actions.Player.Talk.performed += OnTalk;
        actions.UI.Enable();
        actions.UI.Escape.performed += OnEscape;
    }

    

    private void OnDisable()
    {
        actions.UI.Escape.performed -= OnEscape;
        actions.UI.Disable();
        actions.Player.Talk.performed -= OnTalk;
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
    }

    // 체력 만들고
    // 맞을때 어떻게 할지 생각해야하고
    // 맞을때 캐릭 잠깐동안 깜빡이고 무적상태
    // 뒤로 밀려남 

    // esc눌렀을때 일시정지 및 옵션 생성
    // 

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
        Collider2D[] col = Physics2D.OverlapCircleAll(transform.position, 1.5f, LayerMask.GetMask("Npc"));

        if (col.Length > 0)
        {
            scanObject = col[0].gameObject;
        }
        else
        {
            Debug.Log("대상이 없습니다.");
        }
    }

    // 자기가 보고있는 방향으로 공격하기
    private void OnAttack(InputAction.CallbackContext context)
    {
        Debug.Log("공격");
        anim.SetTrigger("Attack");
    }



    private void OnEscape(InputAction.CallbackContext obj)
    {
        Debug.Log("메뉴");
    }

    private void OnTalk(InputAction.CallbackContext obj)
    {
        SearchNpc();

        if (scanObject != null)
        {
            AskAction(scanObject);

        }
        else
        {
            Debug.Log("대상이 없습니다.");
        }
    }

    void AskAction(GameObject scanObj)
    {
        scanObject = scanObj;
        ObjectData objData = scanObject.GetComponent<ObjectData>();
        Talk(objData.id, objData.isNpc);

        //대화창 온
        GameManager.Inst.TalkPanel.SetActive(isAction);
    }

    void Talk(int id, bool isNpc)
    {
        isAction = false;

        //QuestManager의 GetQuestTalkIndex의 파라미터에 전달할 questTalkIndex
        //GetQuestTalkIndex는 questId 값을 리턴
        int questTalkIndex = QuestManager.Instance.GetQuestTalkIndex(id);
        
        //questId를 
        string talkData = GameManager.Inst.talkManager.GetTalk(id + questTalkIndex, QuestManager.Instance.TalkIndex);

        //말할 때 못 움직이게
        float sspeed = 5.0f;
        float speed = 0;

        if (isNpc)
        {
            GameManager.Inst.talkText.text = talkData;
        }
        else
        {
            GameManager.Inst.talkText.text = talkData;
        }

        //대화끝
        if (talkData == null)
        {
            isAction = false;
            QuestManager.Instance.TalkIndex = 0;
            Debug.Log(QuestManager.Instance.CheckQuest(id));
            moveSpeed = sspeed;

            return;
        }

        isAction = true;
        QuestManager.Instance.TalkIndex++;
        moveSpeed = speed;
    }
}

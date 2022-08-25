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

    public float AttackPower{ get => attackPower; }

    public float Defence { get => defencePower;}

    public float CriticalRate { get => criticalRate; }


    //--------------------------------------------------------------------------------

    public GameManager manager;
    GameObject scanObject;
    GameObject sword;

    PlayerInputActions actions;
    Animator anim;
    Rigidbody2D rigid = null;
    CapsuleCollider2D Collider;
    bool isAction = false;
    ItemInventory_UI inven;

    public GameObject shootPrefab = null;
    public float moveSpeed = 5.0f;
    public float itemPickupRange = 1.0f;

    private Vector3 direction = Vector3.zero;


    public PlayerInputActions Actions => actions;

    // Inventory ---------------------------------------------
    ItemInventory_UI invenUI;

    private void Awake()
    {
        actions = new();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        Collider = GetComponent<CapsuleCollider2D>();
        invenUI = FindObjectOfType<ItemInventory_UI>();
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

    private void Start()
    {
        Inventory inven = new Inventory();
        invenUI.InitializeInven(inven);

        inven.AddItem(ItemID.HPPotion);
        inven.AddItem(ItemID.HPPotion);
        inven.AddItem(ItemID.HPPotion);
        inven.AddItem(ItemID.ManaPotion);
        inven.AddItem(ItemID.HPPotion);
        inven.AddItem(ItemID.HPPotion, 3);
        inven.AddItem(ItemID.HPPotion, 3);
        inven.AddItem(ItemID.HPPotion, 3);
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

        if(Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            OnPickup();
        }
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
        Collider2D[] col = Physics2D.OverlapCircleAll(transform.position, 1.5f, LayerMask.GetMask("Npc"));

        if (col.Length > 0)
        {
            scanObject = col[0].gameObject;
        }
        else
        {
            //Debug.Log("대상이 없습니다.");
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


    public void StatusUpdate(Weapon_Item weapon)
    {
        float defaultAttack = 30;
        float defaultDefence = 10;
        float defaultCritical = 0.3f;

        GameManager.Inst.MainPlayer.attackPower = defaultAttack;
        GameManager.Inst.MainPlayer.defencePower = defaultDefence;
        GameManager.Inst.MainPlayer.criticalRate = defaultCritical;

        GameManager.Inst.MainPlayer.attackPower += weapon.data.attackPower;
        GameManager.Inst.MainPlayer.defencePower += weapon.data.defencePower;
        GameManager.Inst.MainPlayer.criticalRate += weapon.data.criticalRate;

        Debug.Log($"공격력 : {GameManager.Inst.MainPlayer.attackPower}");
        Debug.Log($"방어력 : {GameManager.Inst.MainPlayer.defencePower}");
        Debug.Log($"크리율 : {GameManager.Inst.MainPlayer.criticalRate}");
    }


    /// <summary>
    /// 키를 누르면 주위에 있는 아이템을 인벤토리에 추가한다.
    /// </summary>
    public void OnPickup()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, itemPickupRange, LayerMask.GetMask("Items"));
        
        foreach (var col in cols)
        {
            Items item = col.gameObject.GetComponent<Items>();


            inven.Inven.AddItem(item.data);
            
            //Destroy(col.gameObject);
            
        }
    }



}

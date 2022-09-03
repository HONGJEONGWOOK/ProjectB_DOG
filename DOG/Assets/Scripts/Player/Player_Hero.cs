using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine.UI;



public class Player_Hero : MonoBehaviour, IHealth, IBattle
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

    public Puzzle pz;
    public MiniPuzzle mpz;

    bool isAction = false;


    public GameObject shootPrefab = null;
    public float moveSpeed = 15.0f;
    public float itemPickupRange = 1.0f;

    private Vector3 direction = Vector3.zero;

    public Vector3 Direction => direction;
    public PlayerInputActions Actions => actions;

    //----------------------------------
    Transform weaponOfPlayer;
    //public WeaponUI weaponUI;
    Weapon_Item defaultWeapon;

    // Inventory ---------------------------------------------
    ItemInventory_UI invenUI;
    // Minimap -----------------------------------------------
    public Transform marker;
    float markerRotation = 0f;

    // Sound --------------------------------------------------
    IEnumerator footstepCoroutine;
    WaitForSeconds footstepWaitSeconds;
    int footstepCounter = 0;
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
        invenUI = FindObjectOfType<ItemInventory_UI>();

        
        //weaponUI = GetComponent<WeaponUI>();
        weaponOfPlayer = transform.Find("Weapon");
        defaultWeapon = weaponOfPlayer.GetComponentInChildren<Weapon_Item>();

        footstepCoroutine = PlayFootStepSound();
        footstepWaitSeconds = new WaitForSeconds(0.3f);
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

        
        StatusUpdate(defaultWeapon);
    }

    private void OnEnable()
    {
        actions.Player.Enable();
        actions.Player.Move.performed += OnMove;
        actions.Player.Move.canceled += OnMove;
        actions.Player.Attack.performed += OnAttack;
        actions.Player.Talk.performed += OnTalk;
        actions.Player.PickUp.performed += OnPickUp;
        actions.UI.Enable();
        actions.UI.Escape.performed += OnEscape;
        actions.WeaponSlotRotation.Enable();
        actions.WeaponSlotRotation.RoatateDirection.performed += OnWeaponChange;
    }



    private void OnDisable()
    {
        actions.WeaponSlotRotation.RoatateDirection.performed -= OnWeaponChange;
        actions.WeaponSlotRotation.Disable();
        actions.UI.Escape.performed -= OnEscape;
        actions.UI.Disable();
        actions.Player.PickUp.performed -= OnPickUp;
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

            // 코루틴이 한 번만 실행되도록
            if (footstepCounter < 1)
            {
                StartCoroutine(footstepCoroutine);
                footstepCounter++;
            }
        }
        else
        {
            anim.SetBool("Input", false);
            StopCoroutine(footstepCoroutine);
            footstepCounter = 0;
        }

        if (direction.x > 0)
        {
            markerRotation = 90f;
        }
        else if (direction.x < 0)
        {
            markerRotation = -90f;
        }
        if (direction.y > 0)
        {
            markerRotation = 180f;
        }
        else if (direction.y < 0)
        {
            markerRotation = 0f;
        }
        marker.rotation = Quaternion.Euler(0, 0, markerRotation);
    }
    // 움직일때 마지막에 봤던 방향으로 멈춰있기

    // 자기가 보고있는 방향으로 공격하기
    private void OnAttack(InputAction.CallbackContext context)
    {
        Debug.Log("공격");
        anim.SetTrigger("Attack");
        SoundManager.Inst.PlaySound(SoundID.SwordSwing, true);
    }



    private void OnEscape(InputAction.CallbackContext obj)
    {
        Debug.Log("메뉴");
        MenuOnOff();

    }

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




    public void MenuOnOff()
    {
        if (manager.menuSet == false)
        {
            manager.menu.gameObject.SetActive(true);
            manager.menuSet = true;
            Time.timeScale = 0;

        }
        else
        {
            manager.menu.gameObject.SetActive(false);
            manager.menuSet = false;
            Time.timeScale = 1;
        }
        SoundManager.Inst.PlaySound(SoundID.windowOpen);
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
        GameManager.Inst.TalkPanel.SetActive(isAction);

        //대화창 온
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
        float sspeed = 15.0f;
        float speed = 0;

        if (isNpc)
        {
            GameManager.Inst.TalkText.text = talkData;
        }
        else
        {
            GameManager.Inst.TalkText.text = talkData;
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


    
    private void OnWeaponChange(InputAction.CallbackContext context)
    {
        
        int input = (int)context.ReadValue<float>();                // 입력값을 int로 변경
        //weaponUI.RotateWeaponUI(input);
        GameManager.Inst.WeaponUI.RotateWeaponUI(input);
        uint currentWeapon = GameManager.Inst.WeaponOfPlayer.currentWeapon(input);
        GameManager.Inst.WeaponOfPlayer.ChangeWeapon(currentWeapon);
    }


    
    public void StatusUpdate(Weapon_Item weapon)
    {
        float defaultAttack = 30;
        float defaultDefence = 10;
        float defaultCritical = 0.3f;

        attackPower = defaultAttack + weapon.data.attackPower;
        defencePower = defaultDefence + weapon.data.defencePower;
        criticalRate = defaultCritical + weapon.data.criticalRate;

        Debug.Log($"공격력 : {attackPower}");
        Debug.Log($"방어력 : {defencePower}");
        Debug.Log($"크리율 : {criticalRate}");
    }


    /// <summary>
    /// 키를 누르면 주위에 있는 아이템을 인벤토리에 추가한다.
    /// </summary>
    private void OnPickUp(InputAction.CallbackContext obj)
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, itemPickupRange, LayerMask.GetMask("Items"));

        foreach (var col in cols)
        {
            Items item = col.gameObject.GetComponent<Items>();


            invenUI.Inven.AddItem(item.data);

            ItemManager.ReturnItem(ItemManager.Inst.PooledItems[item.data.id], col.gameObject);
        }
    }


    IEnumerator PlayFootStepSound()
    {
        while (true)
        {
            SoundManager.Inst.PlaySound(SoundID.playerFootStep, true);
            yield return footstepWaitSeconds;
        }
    }
}

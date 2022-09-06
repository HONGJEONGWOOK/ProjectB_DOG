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
                Debug.Log(hp);
            }
        }
    }

    public float MaxHP => maxHP;

    public System.Action onHealthChange { get; set; }

    //IBattle--------------------------------------------------------------------------------
    [SerializeField] float attackPower = 30.0f;
    public float defencePower = 10.0f;
    public float criticalRate = 0.3f;

    public float AttackPower => attackPower;

    public float Defence => defencePower;

    public float CriticalRate => criticalRate;

    //--------------------------------------------------------------------------------
    GameObject scanObject;

    PlayerInputActions actions;
    Animator anim;
    Rigidbody2D rigid = null;
    SpriteRenderer sprite;

    public Puzzle pz;
    public MiniPuzzle mpz;

    bool isAction = false;
    bool isHit = false;

    public float moveSpeed = 15.0f;
    public float itemPickupRange = 1.0f;

    public uint weaponIndex = 0;

    public GameObject gameOver;

    private Vector3 direction = Vector3.zero;

    public PlayerInputActions Actions => actions;

    //----------------------------------
    WeaponOfPlayer weaponOfPlayer;
    //public WeaponUI weaponUI;
    Weapon_Item defaultWeapon;

    Transform shootPosition;
    float shootOffset = 1.0f;
    float shootPosRotation = 0f;


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

        invenUI = FindObjectOfType<ItemInventory_UI>();
        sprite = GetComponent<SpriteRenderer>();
        
        //weaponUI = GetComponent<WeaponUI>();
        weaponOfPlayer = FindObjectOfType<WeaponOfPlayer>();
        defaultWeapon = weaponOfPlayer.GetComponentInChildren<Weapon_Item>();
        weaponOfPlayer.gameObject.SetActive(false);
        
        shootPosition = transform.Find("ShootPosition");

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
        actions.UI.ItemUse.performed += OnItemUse;
        actions.WeaponSlotRotation.Enable();
        actions.WeaponSlotRotation.RoatateDirection.performed += OnWeaponChange;
    }

    private void OnDisable()
    {
        actions.WeaponSlotRotation.RoatateDirection.performed -= OnWeaponChange;
        actions.WeaponSlotRotation.Disable();
        actions.UI.ItemUse.performed += OnItemUse;
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
        if (isHit == false)
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
                StartCoroutine(OnHit());
                isHit = true;
            }
            else
            {
                Debug.Log("죽음");
                Die();
            }
        }
    }

    private void FixedUpdate()
    {
        Move();
        if(Keyboard.current.digit0Key.wasPressedThisFrame)
        {
            Die();
        }
    }

    // 체력 만들고
    // 맞을때 어떻게 할지 생각해야하고
    // 맞을때 캐릭 잠깐동안 깜빡이고 무적상태
    // 뒤로 밀려남

    // esc눌렀을때 일시정지 및 옵션 생성
    //

    private void Move()
    {
        rigid.MovePosition(transform.position + (moveSpeed * Time.fixedDeltaTime * direction));
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();

        if (direction.x != 0 || direction.y != 0)
        {
            anim.SetBool("Input", true);

            anim.SetFloat("X", direction.x);
            anim.SetFloat("Y", direction.y);

            shootPosition.localPosition = direction * shootOffset;

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
            shootPosRotation = 0f;
        }
        else if (direction.x < 0)
        {
            markerRotation = -90f;
            shootPosRotation = 180f;
        }
        if (direction.y > 0)
        {
            markerRotation = 180f;
            shootPosRotation = 90f;
        }
        else if (direction.y < 0)
        {
            markerRotation = 0f;
            shootPosRotation = -90f;
        }

        marker.rotation = Quaternion.Euler(0, 0, markerRotation);
        shootPosition.rotation = Quaternion.Euler(0f, 0f, shootPosRotation);
    }
    // 움직일때 마지막에 봤던 방향으로 멈춰있기

    // 자기가 보고있는 방향으로 공격하기
    private void OnAttack(InputAction.CallbackContext _)
    {
        anim.SetInteger("WeaponCount", (int)weaponIndex);
        anim.SetTrigger("Attack");

        if (weaponIndex != 2)
        { // ! Arrow
            GetComponent<AudioSource>().volume = SoundManager.Inst.clips[(byte)SoundID.SwordSwing].volume;
            GetComponent<AudioSource>().PlayOneShot(SoundManager.Inst.clips[(byte)SoundID.SwordSwing].clip);
        }
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
        if (GameManager.Inst.menuSet == false)
        {
            GameManager.Inst.menu.gameObject.SetActive(true);
            GameManager.Inst.menuSet = true;
            Time.timeScale = 0;
        }
        else
        {
            GameManager.Inst.menu.gameObject.SetActive(false);
            GameManager.Inst.menuSet = false;
            Time.timeScale = 1;
        }
        SoundManager.Inst.PlaySound(SoundID.windowOpen);
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
        if (isNpc)
        {
            GameManager.Inst.TalkText.text = talkData;
        }
        else
        {
            Debug.Log("Npc가 아닙니다.");
        }

        //대화끝
        if (talkData == null)
        {
            isAction = false;
            QuestManager.Instance.TalkIndex = 0;
            Debug.Log(QuestManager.Instance.CheckQuest(id));
            actions.Player.Move.Enable();
            return;
        }
        
        isAction = true;
        QuestManager.Instance.TalkIndex++;
        actions.Player.Move.Disable();
    }

    public void ShootArrow()
    {
        GameObject obj = EnemyBulletManager.Inst.GetPooledObject(EnemyBulletManager.PooledObjects[(int)ProjectileID.Arrows]);
        obj.transform.position = shootPosition.position;
        obj.transform.right = shootPosition.right;
        obj.SetActive(true);

        GetComponent<AudioSource>().volume = SoundManager.Inst.clips[(byte)SoundID.ShootArrow].volume;
        GetComponent<AudioSource>().PlayOneShot(SoundManager.Inst.clips[(byte)SoundID.ShootArrow].clip);
    }

    private void OnWeaponChange(InputAction.CallbackContext context)
    {
        int input = (int)context.ReadValue<float>();                // 입력값을 int로 변경
        //weaponUI.RotateWeaponUI(input);
        GameManager.Inst.WeaponUI.RotateWeaponUI(input);
        weaponIndex = weaponOfPlayer.currentWeapon(input);
        weaponOfPlayer.ChangeWeapon(weaponIndex);
    }

    public void StatusUpdate(Weapon_Item weapon)
    {
        float defaultAttack = 30;
        float defaultDefence = 10;
        float defaultCritical = 0.3f;

        attackPower = defaultAttack + weapon.data.attackPower;
        defencePower = defaultDefence + weapon.data.defencePower;
        criticalRate = defaultCritical + weapon.data.criticalRate;

        //Debug.Log($"공격력 : {attackPower}");
        //Debug.Log($"방어력 : {defencePower}");
        //Debug.Log($"크리율 : {criticalRate}");
    }

    public void OnItemUse(InputAction.CallbackContext context)
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {// 9번 슬롯
            invenUI.Inven[9].UseSlotItem(this.gameObject);
        }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {// 10번 슬롯
            invenUI.Inven[10].UseSlotItem(this.gameObject);
        }
        else if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {// 11번 슬롯
            invenUI.Inven[11].UseSlotItem(this.gameObject);
        }
        else if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {// 12번 슬롯
            invenUI.Inven[12].UseSlotItem(this.gameObject);
        }
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

    private void Die()
    {
        Debug.Log("죽음");
        QuestManager.Instance.questId = 10;
        QuestManager.Instance.questActionIndex = 0;
        actions.Player.Disable();

        if (gameOver != null)
        {
            gameOver.SetActive(true);
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

    IEnumerator OnHit()
    {
        int countTime = 0;

        while(countTime < 10)
        {
            if (countTime % 2 == 0)
                sprite.color = new Color32(255, 255, 255, 90);
            else
                sprite.color = new Color32(255, 255, 255, 180);

            yield return new WaitForSeconds(0.2f);
            countTime++;
        }

        sprite.color = new Color(255, 255, 255, 255);


        isHit = false;

        yield return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TalkManager talkManager;
    Text talkText;
    public GameObject scanObject;
    public MenuSet menu;
    public bool menuSet = false;
    ItemInventory_UI inventoryUI;

    //퀘스트매니저
    public QuestManager questManager;

    private GameObject talkPanel;
    public GameObject TalkPanel
    {
        get { return talkPanel; }
    }

    public Text TalkText
    {
        get => talkText;
        set
        {
            talkText = value;
        }
    }

    // 플레이어 ----------------------------------------------------------
    Player_Hero player = null;
    public Player_Hero MainPlayer { get => player; }

    // 맵 매니저----------------------------------------------------------
    int oldSceneIndex = 0;

    // 웨폰 매니저--------------------------------------------------
    WeaponDataManager weaponData;
    public WeaponDataManager WeaponData { get => weaponData; }

    ItemDataManager itemData;
    public ItemDataManager ItemData => itemData;


    public ItemInventory_UI InvenUI => inventoryUI;

    public static GameManager Inst { get => instance;}
    static GameManager instance;

    

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Initialize();
            DontDestroyOnLoad(this.gameObject);     // 씬이 변경되더라도 게임 오브젝트가 사라지기 않게 해주는 함수
        }
        else
        {
            // 씬의 Gamemanager가 여러번 생성됐다.
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Start()
    {
        GameLoad();
    }

    void Initialize()
    {
        // --------------- 플레이어
        player = FindObjectOfType<Player_Hero>();

        // --------------- NPC
        talkPanel = transform.GetChild(0).GetChild(1).gameObject;
        talkText = talkPanel.GetComponentInChildren<Text>();
        //talkPanel.SetActive(false);

        //---------------- Inventory
        weaponData = GetComponent<WeaponDataManager>();
        itemData = GetComponent<ItemDataManager>();

        SceneManager.sceneLoaded += OnStageStart;   // 씬의 로딩이 끝났을 때 실행될 델리게이트에 OnStageStart 등록

        inventoryUI = FindObjectOfType<ItemInventory_UI>();
    }

    private void OnStageStart(Scene arg0, LoadSceneMode arg1)
    {

        if (oldSceneIndex != arg0.buildIndex)
        {
            oldSceneIndex = arg0.buildIndex;
        }

        talkPanel.SetActive(false);
    }

    public void GameCountinu()
    {
        player.MenuOnOff();
    }

    public void GameSave()
    {
        //저장할꺼
        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
        PlayerPrefs.Save();



    }

    public void GameLoad()
    {
        if (!PlayerPrefs.HasKey("PlayerX"))
            return;

        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");

        player.transform.position = new Vector3(x, y, 0);
    }

    public void GameExit()
    {
        Application.Quit();
    }
}

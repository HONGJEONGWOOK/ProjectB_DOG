using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TalkManager talkManager;
    public GameObject scanObject;
    public MenuSet menu;
    public bool menuSet = false;
    ItemInventory_UI inventoryUI;

    //퀘스트매니저
    QuestManager questManager;
    public QuestManager questmanager { get => questManager; }

    private GameObject talkPanel;
    public GameObject TalkPanel
    {
        get { return talkPanel; }
    }

    Text talkText;
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

    WeaponOfPlayer weaponOfPlayer = null;
    public WeaponOfPlayer WeaponOfPlayer => weaponOfPlayer;

    WeaponUI weaponUI = null;
    public WeaponUI WeaponUI => weaponUI;   

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


    void Initialize()
    {
        // --------------- NPC
        talkPanel = transform.GetChild(0).GetChild(1).gameObject;
        talkText = talkPanel.GetComponentInChildren<Text>();

        SceneManager.sceneLoaded += OnStageStart;   // 씬의 로딩이 끝났을 때 실행될 델리게이트에 OnStageStart 등록
    }

    private void OnStageStart(Scene arg0, LoadSceneMode arg1)
    {
        // --------------- 플레이어
        player = FindObjectOfType<Player_Hero>();

        //---------------- Inventory
        inventoryUI = FindObjectOfType<ItemInventory_UI>();
        weaponData = GetComponent<WeaponDataManager>();
        weaponUI = FindObjectOfType<WeaponUI>();
        weaponOfPlayer = FindObjectOfType<WeaponOfPlayer>();
        itemData = GetComponent<ItemDataManager>();
        questManager = GetComponent<QuestManager>();

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

    public void GameExit()
    {
        Application.Quit();
    }

    public void GameRestart()
    {
        LoadingSceneManager.LoadScene(0);
    }
}

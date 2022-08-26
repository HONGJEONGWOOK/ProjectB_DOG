using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //퀘스트매니저
    public TalkManager talkManager;
    public QuestManager questManager;

    private GameObject talkPanel;
    public GameObject TalkPanel
    {
        get { return talkPanel; }
    }

    public Text talkText;
    public GameObject scanObject;
    
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

    ItemInventory_UI inventoryUI;
    public ItemInventory_UI InvenUI => inventoryUI;

    public static GameManager Inst { get => instance;}
    static GameManager instance = null;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            instance.Initialize();
            DontDestroyOnLoad(this.gameObject);     // 씬이 변경되더라도 게임 오브젝트가 사라지기 않게 해주는 함수
        }
        else
        {
            // 씬의 Gamemanager가 여러번 생성됐다.
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void Initialize()
    {
        // --------------- 플레이어 
        player = FindObjectOfType<Player_Hero>();

        // --------------- NPC
        talkPanel = GameObject.Find("TalkPanel");
        talkPanel.SetActive(false);

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

    }
}

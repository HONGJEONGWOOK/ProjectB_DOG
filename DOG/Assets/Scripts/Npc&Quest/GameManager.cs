using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TalkManager talkManager;
    public QuestManager questManager;
    public GameObject talkPanel;
    public Text talkText;
    public GameObject scanObject;

    //퀘스트매니저


    public bool isAction;
    public int talkIndex;

    // 맵 매니저-----------------------------------------------------------\
    int oldSceneIndex = 0;

    public static GameManager Inst { get => instance;}
    static GameManager instance = null;

    private void Start()
    {
        //Debug.Log(questManager.CheckQuest());
    }

    public void Awake()
    {
        talkPanel.SetActive(false);

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
        SceneManager.sceneLoaded += OnStageStart;   // 씬의 로딩이 끝났을 때 실행될 델리게이트에 OnStageStart 등록
    }

    private void OnStageStart(Scene arg0, LoadSceneMode arg1)
    {

        if (oldSceneIndex != arg0.buildIndex)
        {
            oldSceneIndex = arg0.buildIndex;
        }

    }


    //게임 오브젝트 타입에 scanObj파라미터로 받아서 GameObject타입형의 scanObject에 넣고
    //scanObject에 GetComponent해서 기능을 ObjectData타입의 objData에 넣고
    //ObjectData타입은 public int id; public bool isNpc; 를 가지고 있음
    //
    public void AskAction(GameObject scanObj)
    {
        scanObject = scanObj;
        ObjectData objData = scanObject.GetComponent<ObjectData>();
        Talk(objData.id, objData.isNpc);

        //대화창 온
        talkPanel.SetActive(isAction);
    }

    void Talk(int id, bool isNpc)
    {
        //QuestManager의 GetQuestTalkIndex의 파라미터에 전달할 questTalkIndex
        //GetQuestTalkIndex는 questId 값을 리턴
        int questTalkIndex = questManager.GetQuestTalkIndex(id);

        //questId를 
        string talkData = talkManager.GetTalk(id + questTalkIndex, talkIndex);

        if (isNpc)
        {
            talkText.text = talkData;
        }
        else
        {
            talkText.text = talkData;
        }

        //대화끝
        if (talkData == null)
        {
            isAction = false;
            talkIndex = 0;
            Debug.Log(questManager.CheckQuest(id));
            return;
        }

        isAction = true;
        talkIndex++;
    }
}

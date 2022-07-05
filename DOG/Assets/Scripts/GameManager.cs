using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TalkManager talkManager;
    public GameObject talkPanel;
    public Text talkText;
    public GameObject scanObject;

    //퀘스트매니저
    public QuestManager questManager;

    public bool isAction;
    public int talkIndex;

    private void Start()
    {
        Debug.Log(questManager.CheckQuest());
    }

    public void Awake()
    {
        talkPanel.SetActive(false);
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

        if(isNpc)
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

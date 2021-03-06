using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//퀘스트 넘버를 관리할 스크립트
public class QuestManager : MonoBehaviour
{
    private static QuestManager instance;
    public static QuestManager Instance {get {return instance;}}

    //몬스터 매니저
    Monsters monsters;

    //퀘스트 아이디
    public int questId;
    public int questActionIndex;

    public GameObject[] questObject;

    //퀘스트 데이터를 불러올 리스트
    Dictionary<int, QuestData> questList;

    private int goblinQuestCount = 0;

    int talkIndex;
    public int TalkIndex
    {
        get {return talkIndex;}
        set {talkIndex = value;}
    }

    private void Awake()
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
        //게임 시작할 때 어웨이크로 리스트 불러와줌
        //questList를 사용하기 위해 초기화 해주고
        questList = new Dictionary<int, QuestData>();

        //이 함수를 통해서 실행함
        GenerateData();
    }

    //초기화된 questList를 사용하기 위한 함수
    private void GenerateData()
    {
        //리스트에 Add 명령어로 더하는 것은 동일
        //10 - 퀘스트 고유 넘버, 퀘스트데이타는 퀘스트 명과 관련된 엔피시 번호를 받아옴?
        questList.Add(10, new QuestData("사라진 마을 주민들", new int[] { 1000, 2000 }));

        questList.Add(20, new QuestData("갑자기 꽃?", new int[] { 3000, 2000 }));

        questList.Add(30, new QuestData("마을의 위험요소 처치", new int[] { 2000, 1000}));
        //4000 - 몬스터 번호
        questList.Add(40, new QuestData("몬스터 처치 완료", new int[] { 1000, 2000 }));

        questList.Add(50, new QuestData("의심되는 곳 수색", new int[] { 0 }));
    }


    //
    public int GetQuestTalkIndex(int id)
    {
        return questId + questActionIndex;
    }

    public string CheckQuest(int id)
    {
        //다음 토크 타겟
        if (id == questList[questId].npcId[questActionIndex])
        {
            questActionIndex++;
        }

        //퀘스트오브젝트 컨트롤
        ControlObject();

        //대화 완료 및 다음 퀘스트
        if (questActionIndex == questList[questId].npcId.Length)
        {
            NextQuest();
        }

        return questList[questId].questName;
    }

    //고블린 죽은 횟수를 감지하여 넘겨줄 함수, 몬스터 쪽 데이터와 연동 필요
    //public int GoblinKillCountCheck()
    //{
    //    int count =  monsters.DeadCount;
    //
    //    return count;
    //}    

    public string CheckQuest()
    {
        return questList[questId].questName;
    }

    void NextQuest()
    {
        questId += 10;
        questActionIndex = 0;
    }

    void ControlObject()
    {
        switch(questId)
        {
            //10번 퀘스트를 끝나치면 열쇠 꽃 등장
            case 10:
                if(questActionIndex == 2)
                {
                    questObject[0].SetActive(true);
                }
                break;
            
            //20번 퀘스트에서 1번째 순서가 끝나고 꽃을 먹으면 사라짐
            case 20:
                if(questActionIndex == 1)
                {
                    questObject[0].SetActive(false);
                }
                break;

            case 40:
                if(questActionIndex == 1)
                {
                    questObject[1].SetActive(false);
                }
                break;
        }
    }
}

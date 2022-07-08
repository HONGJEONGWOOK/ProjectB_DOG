using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//퀘스트 넘버를 관리할 스크립트
public class QuestManager : MonoBehaviour
{
    //퀘스트 아이디
    public int questId;
    public int questActionIndex;

    public GameObject[] questObject;

    //퀘스트 데이터를 불러올 리스트
    Dictionary<int, QuestData> questList;

    
    private void Awake()
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

        questList.Add(30, new QuestData("마을의 위험요소 처치", new int[] { 2000, 1000 }));

        questList.Add(40, new QuestData("몬스터들은 어디서 왔을까", new int[] { 0 }));

    }


    //
    public int GetQuestTalkIndex(int id)
    {
        return questId + questActionIndex;
    }

    public string CheckQuest(int id)
    {

        if (id == questList[questId].npcId[questActionIndex])
        {
            questActionIndex++;
        }

        //퀘스트오브젝트 컨트롤
        ControlObject();

        if (questActionIndex == questList[questId].npcId.Length)
        {
            NextQuest();
        }

        return questList[questId].questName;
    }

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
            case 10:
                if(questActionIndex == 2)
                {
                    questObject[0].SetActive(true);
                }
                break;
            case 20:
                if(questActionIndex == 1)
                {
                    questObject[0].SetActive(false);
                }
                break;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static UnityEngine.Rendering.VolumeComponent;

//퀘스트 넘버를 관리할 스크립트
public class QuestManager : MonoBehaviour
{
    private static QuestManager instance;
    public static QuestManager Instance { get { return instance; } }

    //퀘스트 아이디
    public int questId;
    public int questActionIndex;

    public int killCount = 0;
    public static System.Action CheckKillCount;

    public int bosskillCount = 0;
    public static System.Action BossKillCount;

    //퀘스트 데이터를 불러올 리스트
    Dictionary<int, QuestData> questList;

    int talkIndex;
    public int TalkIndex
    {
        get { return talkIndex; }
        set { talkIndex = value; }
    }

    AudioSource audioSource;

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

    private void Update()
    {
        
    }
    void Initialize()
    {
        //게임 시작할 때 어웨이크로 리스트 불러와줌
        //questList를 사용하기 위해 초기화 해주고
        questList = new Dictionary<int, QuestData>();

        //이 함수를 통해서 실행함
        GenerateData();

        //패널 끄고 열고 오디오
        audioSource = GetComponent<AudioSource>();

        //고블린 킬수 체크용 델리게이트
        CheckKillCount = () => { checkkillcount(); };

        //보스 킬 체크용 델리게이트
        BossKillCount = () => { BossskillCount(); };

    }

    //초기화된 questList를 사용하기 위한 함수
    private void GenerateData()
    {
        //리스트에 Add 명령어로 더하는 것은 동일
        //10 - 퀘스트 고유 넘버, 퀘스트데이타는 퀘스트 명과 관련된 엔피시 번호를 받아옴?
        questList.Add(10, new QuestData("사라진 마을 주민들", new int[] { 1000, 2000 }));

        questList.Add(20, new QuestData("재앙의 원인을 찾아라", new int[] { 1000, 2000 }));

        questList.Add(30, new QuestData("마을의 위험요소 처치", new int[] { 2000, 1000 }));

        questList.Add(40, new QuestData("몬스터 처치 완료", new int[] { 1000, 3000 }));

        questList.Add(50, new QuestData("마법의 공간을 빠져나가자", new int[] { 2000, 3000 }));

        questList.Add(60, new QuestData("상자 확보", new int[] { 2000, 3000 }));

        questList.Add(70, new QuestData("장로 처치", new int[] { 2000 }));

        questList.Add(80, new QuestData("다시 찾아온 평화", new int[] { 2000 }));
    }

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

        //대화 완료 및 다음 퀘스트
        if (questActionIndex == questList[questId].npcId.Length)
        {
            NextQuest();
        }

        QuestObject();
        return questList[questId].questName;
    }

    public string CheckQuest()
    {
        return questList[questId].questName;
    }

    public void NextQuest()
    {
        questId += 10;
        questActionIndex = 0;
    }

    void QuestObject()
    {
        if (questId == 30 && questActionIndex == 0)
        {
            if (killCount == 5)
            {
                NextQuest();
                killCount = 0;
            }
        }
        if (questId == 40 && questActionIndex == 1)
        {
            GameObject obj = GameObject.Find("VillageElder");
            Destroy(obj, 0.5f);
        }
        else if (questId == 50 && questActionIndex == 0)
        {
            LoadingSceneManager.LoadScene(5);
        }
        else if (questId == 70 && questActionIndex == 0)
        {
            GameObject objj = GameObject.Find("MiniGameBox");
            Destroy(objj);

            
        }
        else if(questId == 80 && questActionIndex == 0)
        {
            if (bosskillCount == 1)
            {
                NextQuest();
                bosskillCount = 0;
            }
        }
    }

    void checkkillcount()
    {
        killCount++;
    }

    void BossskillCount()
    {
        bosskillCount++;
    }
}
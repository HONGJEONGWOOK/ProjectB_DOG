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

    //퀘스트 데이터를 불러올 리스트
    Dictionary<int, QuestData> questList;

    int talkIndex;
    public int TalkIndex
    {
        get { return talkIndex; }
        set { talkIndex = value; }
    }

    AudioSource audioSource;

    QuestPanel questPanel;
    public int killCount = 0;
    public static System.Action CheckKillCount;

    public int bosskillCount = 0;
    public static System.Action BossKillCount;

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
        if (questId == 30 && questActionIndex == 0)
        {
            questPanel.qeustName.text = "마을 밖 순찰 및 고블린 처치!";
            questPanel.qeustDetail.text = $"마을 밖을 순찰하며 고블린을 5마리 처치하자.\n고블린 킬수 : {killCount}";
            if (killCount == 5)
            {
                NextQuest();
                questPanel.qeustName.text = "마을 순찰 및 고블린 처치 완료";
                questPanel.qeustDetail.text = "고블린 5마리를 처치하며 순찰을 완료했다. 장로에게 돌아가보자";
                killCount = 0;
            }
        }
        else if (questId == 40 && questActionIndex == 1)
        {
            questPanel.qeustName.text = "장로가 자리비운 시간";
            questPanel.qeustDetail.text = "장로회관에 가루와 관련된 물건이 있는지 한 번 둘러보자";
        }
        else if (questId == 70 && questActionIndex == 0)
        {
            if (bosskillCount == 1)
            {
                NextQuest();
                questPanel.qeustName.text = "재앙 제거 완료";
                questPanel.qeustDetail.text = "영생을 찾아 몬스터가 되어버린 장로를 제거했다....\n다시 마을로 돌아가자.";
                bosskillCount = 0;
            }
        }
    }
    void Initialize()
    {
        //게임 시작할 때 어웨이크로 리스트 불러와줌
        //questList를 사용하기 위해 초기화 해주고
        questList = new Dictionary<int, QuestData>();
        //questPanel = FindObjectOfType<QuestPanel>();
        Debug.Log($"{questId},{questActionIndex}");

        //이 함수를 통해서 실행함
        GenerateData();

        //패널 끄고 열고 오디오
        audioSource = GetComponent<AudioSource>();

        //고블린 킬수 체크용 델리게이트
        CheckKillCount = () => { checkkillcount(); };

        //보스 킬 체크용 델리게이트
        BossKillCount = () => { BossskillCount(); };

        //시작 시 패널 갱신
        questPanel = FindObjectOfType<QuestPanel>();
        Panel(questId, questActionIndex);
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
            Panel(questId, questActionIndex);
        }

        //대화 완료 및 다음 퀘스트
        if (questActionIndex == questList[questId].npcId.Length)
        {
            NextQuest();
            Panel(questId, questActionIndex);
        }

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

    public void Panel(int id, int index)
    {

        if (id == 10 && index == 0)
        {
            questPanel.qeustName.text = "사라진 마을 사람들";
            questPanel.qeustDetail.text = "당신은 병에서 깨어나 눈을 떴습니다. 마을 장로를 찾아가세요.";
        }
        else if (id == 10 && index == 1)
        {
            questPanel.qeustName.text = "사라진 마을 사람들";
            questPanel.qeustDetail.text = "다른 생존자는 없을까? 마을의 집을 둘러보자!";
        }
        else if (id == 20 && index == 0)
        {
            questPanel.qeustName.text = "장로에게 다시 가보자";
            questPanel.qeustDetail.text = "재앙의 원인을 찾아야 한다. 장로에게 다시 돌아가자.";
        }
        else if (id == 20 && index == 1)
        {
            questPanel.qeustName.text = "생존자에게 다시 돌아가보자";
            questPanel.qeustDetail.text = "주민의 말처럼 장로에게 약물을 받았다. 우선 주민에게 상황을 말해주자.";

        }
        else if (id == 40 && index == 1)
        {
            GameObject obj = GameObject.Find("VillageElder");
            Destroy(obj, 0.5f);
            questPanel.qeustName.text = "장로가 자리비운 시간";
            questPanel.qeustDetail.text = "장로회관에 가루와 관련된 물건이 있는지 한 번 둘러보자";
        }
        else if (id == 50 && index == 0)
        {
            questPanel.qeustName.text = "마법이 담긴 함정 상자";
            questPanel.qeustDetail.text = "상자를 열었더니 이상한 공간으로 빨려들어왔다. 퍼즐을 풀어 탈출하자";
            LoadingSceneManager.LoadScene(5);
        }
        else if (id == 60 && index == 0)
        {
            questPanel.qeustName.text = "확실해진 재앙의 원인";
            questPanel.qeustDetail.text = "유일하게 남은 주민에게 이 사실을 알리자";
        }
        else if (questId == 60 && questActionIndex == 1)
        {
            questPanel.qeustName.text = "장로를 찾아라";
            questPanel.qeustDetail.text = "사실을 알렸으니 이제 마을 회관에서 상자를 회수하고 장로를 기다리자.";
        }
        else if (questId == 70 && questActionIndex == 0)
        {
            questPanel.qeustName.text = "장로를 제거하라";
            questPanel.qeustDetail.text = "마을 근처....옛 놀이터였던 땅굴 입구를 찾아 들어가자.";
            GameObject objj = GameObject.Find("MiniGameBox");
            Destroy(objj);
        }
        else if (id == 80 && index == 1)
        {
            questPanel.qeustName.text = "다시 찾아온 평화";
            questPanel.qeustDetail.text = "평화로운 마을을 다시 재건하자.";
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
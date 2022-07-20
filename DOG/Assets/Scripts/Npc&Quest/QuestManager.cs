using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����Ʈ �ѹ��� ������ ��ũ��Ʈ
public class QuestManager : MonoBehaviour
{
    //���� �Ŵ���
    Monsters monsters;

    //����Ʈ ���̵�
    public int questId;
    public int questActionIndex;

    public GameObject[] questObject;

    //����Ʈ �����͸� �ҷ��� ����Ʈ
    Dictionary<int, QuestData> questList;

    
    private void Awake()
    {
        //���� ������ �� �����ũ�� ����Ʈ �ҷ�����
        //questList�� ����ϱ� ���� �ʱ�ȭ ���ְ�
        questList = new Dictionary<int, QuestData>();

        //�� �Լ��� ���ؼ� ������
        GenerateData();
    }

    //�ʱ�ȭ�� questList�� ����ϱ� ���� �Լ�
    private void GenerateData()
    {
        //����Ʈ�� Add ��ɾ�� ���ϴ� ���� ����
        //10 - ����Ʈ ���� �ѹ�, ����Ʈ����Ÿ�� ����Ʈ ��� ���õ� ���ǽ� ��ȣ�� �޾ƿ�?
        questList.Add(10, new QuestData("����� ���� �ֹε�", new int[] { 1000, 2000 }));

        questList.Add(20, new QuestData("���ڱ� ��?", new int[] { 3000, 2000 }));

        questList.Add(30, new QuestData("������ ������ óġ", new int[] { 2000, 1000}));
        //4000 - ���� ��ȣ
        questList.Add(40, new QuestData("���� óġ �Ϸ�", new int[] { 1000, 2000 }));

        questList.Add(50, new QuestData("�ǽɵǴ� �� ����", new int[] { 0 }));
    }


    //
    public int GetQuestTalkIndex(int id)
    {
        return questId + questActionIndex;
    }

    public string CheckQuest(int id)
    {
        //���� ��ũ Ÿ��
        if (id == questList[questId].npcId[questActionIndex])
        {
            questActionIndex++;
        }

        //����Ʈ������Ʈ ��Ʈ��
        ControlObject();

        //��ȭ �Ϸ� �� ���� ����Ʈ
        if (questActionIndex == questList[questId].npcId.Length)
        {
            NextQuest();
        }

        ////��� ���� Ƚ���� üũ�Ͽ� +10���ִ� �Լ��� �ѱ�
        //if(GoblinKillCountCheck() == 5)
        //{
        //    TalkManager talkManager = GetComponent<TalkManager>();
        //    talkManager.QuestNumber1 = 41;
        //}

        //����Ʈ �̸� ����
        return questList[questId].questName;
    }

    ////��� ���� Ƚ���� �����Ͽ� �Ѱ��� �Լ�, ���� �� �����Ϳ� ���� �ʿ�
    //public int GoblinKillCountCheck()
    //{
    //    Monsters monsters = GetComponent<Monsters>();
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
            //10�� ����Ʈ�� ����ġ�� ���� �� ����
            case 10:
                if(questActionIndex == 2)
                {
                    questObject[0].SetActive(true);
                }
                break;
            
            //20�� ����Ʈ���� 1��° ������ ������ ���� ������ �����
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

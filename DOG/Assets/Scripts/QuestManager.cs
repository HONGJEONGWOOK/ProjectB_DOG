using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����Ʈ �ѹ��� ������ ��ũ��Ʈ
public class QuestManager : MonoBehaviour
{
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
        questList.Add(10, new QuestData("���ڱ� ��Ÿ�� ���͵��� �ǽɽ�����!", new int[] { 1000, 2000 }));

        questList.Add(20, new QuestData("������ �����, �ֹ�1", new int[] { 2000, 3000 }));

        questList.Add(30, new QuestData("���� ȹ���ߴ�.", new int[] { 0 }));

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

        //����Ʈ������Ʈ ��Ʈ��
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
                if(questActionIndex == 2)
                {
                    questObject[0].SetActive(false);
                }
                break;
        }
    }
}

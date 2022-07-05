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

    //����Ʈ�Ŵ���
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

    //���� ������Ʈ Ÿ�Կ� scanObj�Ķ���ͷ� �޾Ƽ� GameObjectŸ������ scanObject�� �ְ�
    //scanObject�� GetComponent�ؼ� ����� ObjectDataŸ���� objData�� �ְ�
    //ObjectDataŸ���� public int id; public bool isNpc; �� ������ ����
    //
    public void AskAction(GameObject scanObj)
    {
        scanObject = scanObj;
        ObjectData objData = scanObject.GetComponent<ObjectData>();
        Talk(objData.id, objData.isNpc);

        //��ȭâ ��
        talkPanel.SetActive(isAction);
    }

    void Talk(int id, bool isNpc)
    {
        //QuestManager�� GetQuestTalkIndex�� �Ķ���Ϳ� ������ questTalkIndex
        //GetQuestTalkIndex�� questId ���� ����
        int questTalkIndex = questManager.GetQuestTalkIndex(id);

        //questId�� 
        string talkData = talkManager.GetTalk(id + questTalkIndex, talkIndex);

        if(isNpc)
        {
            talkText.text = talkData;
        }
        else
        {
            talkText.text = talkData;
        }

        //��ȭ��
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

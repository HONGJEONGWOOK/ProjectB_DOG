using System.Collections;
using System.Collections.Generic;

public class QuestData
{
    public string questName; //����Ʈ �̸�
    public int[] npcId;      //����Ʈ�� ������ npc�� id�� �����ϴ� ��Ʈ�迭

    public QuestData(string name, int[] npc)
    {
        questName = name;
        npcId = npc;
    }
}

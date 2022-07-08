using System.Collections;
using System.Collections.Generic;

public class QuestData
{
    public string questName; //퀘스트 이름
    public int[] npcId;      //퀘스트와 연관된 npc의 id를 저장하는 인트배열

    public QuestData(string name, int[] npc)
    {
        questName = name;
        npcId = npc;
    }
}

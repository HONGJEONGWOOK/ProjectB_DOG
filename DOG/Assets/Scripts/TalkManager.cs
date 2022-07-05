using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;

    private void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    private void GenerateData()
    {

        //일반 대화
        talkData.Add(1000, new string[] { "안녕?", "너는?...어디있었던 거니?", "아 그건 중요하지 않고...", "얼른 마을을 떠나기 바라" });

        talkData.Add(2000, new string[] {"당신이 병에 걸려 잠들어 있는 동안", "마을에 안 좋은 일들이 많이 일어났습니다.", "주민들도 떠나고 없는 상태이니 당신도 떠나세요."});

        //퀘스트 대화
        talkData.Add(10 + 1000, new string[] {"장로 : 주민들이 떠난 이유?....", "장로 : 음...", "요즘 마을 주변에 몬스터가 많아", "장로 : 그게 가장 큰 걱정이었어", "독백 : 몬스터들을 처치해야겠구나...다른 주민 말을 걸어보자"});
        talkData.Add(11 + 2000, new string[] { "주민 : 장로가 그렇게 말했다고?", "주민 : 난 생각이 좀 달라", "주민 : 흔적도 없이 다들 사라졌다고!", "독백 : (음...이유가 뭘까)"});

        talkData.Add(20 + 2000, new string[] { "꽃을 구해다 주면 내가 본 것을 말해줄게...." });
        talkData.Add(21 + 3000, new string[] { "이 꽃을 가져가자." }); //여기서 안 넘어감
        talkData.Add(22 + 2000, new string[] { "고마워...", "사실 나도 정확히 본 것은 아니지만 장로에게 비밀이 있는 것 같아", "우선....마을을 위협하는 몬스터들을 해치우는게 어떨까?" });
    }

    public string GetTalk(int id, int talkIndex)
    {
        if(!talkData.ContainsKey(id))
        {
            if (!talkData.ContainsKey(id - id % 10))
            {
                if (talkIndex == talkData[id - id % 100].Length)
                {
                    return null;
                }
                else
                {
                    return talkData[id - id % 100][talkIndex];
                }
            }
            else
            {
                //퀘스트 진행 중 대사가 없을 때 
                //퀘스트 맨 처음 대사를 가지고 옴
                if (talkIndex == talkData[id - id % 10].Length)
                {
                    return null;
                }
                else
                {
                    return talkData[id - id % 10][talkIndex];
                }
            }
        }

        if (talkIndex == talkData[id].Length)
        {
            return null;
        }
        else
        {
            return talkData[id][talkIndex];
        }
    }
}

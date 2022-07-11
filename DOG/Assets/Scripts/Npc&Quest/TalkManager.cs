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

        talkData.Add(100, new string[] { "평범한 바위이다." });

        //퀘스트 대화
        talkData.Add(10 + 1000, new string[] { "장로 : 주민들이 떠난 이유?....", "장로 : 음...", "장로 : 요즘 마을 주변에 몬스터가 많아", "장로 : 그게 가장 큰 걱정이었어", "독백 : 몬스터들을 처치해야겠구나...다른 주민에게 말을 걸어보자"});
        talkData.Add(11 + 2000, new string[] { "주민 : 장로가 그렇게 말했다고?", "주민 : 난 생각이 좀 달라", "주민 : 흔적도 없이 다들 사라졌다고!", "독백 : (음...이유가 뭘까)"});

        talkData.Add(20 + 2000, new string[] { "주민 : 바쁘지 않으면 꽃을 하나 구해줄 수 있니?" });
        talkData.Add(20 + 3000, new string[] { "독백 : 이 꽃을 가져가자." }); //npc와 대화를 하는 것처럼 꽃도 대화를 걸어서 퀘스트를 완료하는 방식으로 함
        talkData.Add(21 + 2000, new string[] { "주민 : 고마워...", "주민 : 증거는 없지만...장로에게 비밀이 있는 것 같아" });

        talkData.Add(30 + 2000, new string[] { "주민 : 우선....몬스터들을 해치우는게 어떨까?" });
        talkData.Add(31 + 1000, new string[] { "장로 : 몬스터 처치는 아직이야?", "독백 : 몬스터를 처치해보자..."});
        talkData.Add(32 + 4000, new string[] { "고블린 : 크윽, 분하다!", "고블린 : 하지만 그 사람이 돕는 한 우리는 안 사리진다고 키키킥", " 독백 : 그 사람이 누구지?"});
        //다섯 번째 고블린에 id 4000을 부여한다음 해치우고 말을 거는 방식을 생각 중
        //대화를 마치면 ++ 되서 33으로 넘어가게.
        talkData.Add(33 + 2000, new string[] { "주민 : 몬스터들을 해치웠구나 정말 대단해!", "" });
        talkData.Add(34 + 1000, new string[] { "장로 : 엇...정말 몬스터들을 해치운거야?...", "일단 나도 원인을 알아보고 있을 테니 좀 쉬고 있을레?", "독백 : 장로의 집을 수색해보자...."});
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
            //id로 대화 talkIndex로 대화의 한 문장을 리턴
            return talkData[id][talkIndex];
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class QuestPanel : MonoBehaviour
{
    public TextMeshProUGUI qeustName;
    public TextMeshProUGUI qeustDetail;

    int id;
    int index;

    private void Awake()
    {
        qeustName = transform.Find("QuestName").GetComponent<TextMeshProUGUI>();
        qeustDetail = transform.Find("QuestDetail").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        Panel();
    }

    public void Panel()
    {

        int id = QuestManager.Instance.questId;
        int index = QuestManager.Instance.questActionIndex;

        if (id == 10 && index == 0)
        {
            qeustName.text = "사라진 마을 사람들";
            qeustDetail.text = "당신은 병에서 깨어나 눈을 떴습니다. 마을 장로를 찾아가세요.";
        }
        else if (id == 10 && index == 1)
        {
            qeustName.text = "사라진 마을 사람들";
            qeustDetail.text = "다른 생존자는 없을까? 마을의 집을 둘러보자!";
        }
        else if (id == 20 && index == 0)
        {
            qeustName.text = "장로에게 다시 가보자";
            qeustDetail.text = "재앙의 원인을 찾아야 한다. 장로에게 다시 돌아가자.";
        }
        else if (id == 20 && index == 1)
        {
            qeustName.text = "생존자에게 다시 돌아가보자";
            qeustDetail.text = "주민의 말처럼 장로에게 약물을 받았다. 우선 주민에게 상황을 말해주자.";

        }
        if (id == 30 && index == 0)
        {
            qeustName.text = "마을 밖 순찰 및 고블린 처치!";
            qeustDetail.text = $"마을 밖을 순찰하며 고블린을 5마리 처치하자.\n고블린 킬수 : {QuestManager.Instance.killCount}";
            if (QuestManager.Instance.killCount == 5)
            {
                qeustName.text = "마을 순찰 및 고블린 처치 완료";
                qeustDetail.text = "고블린 5마리를 처치하며 순찰을 완료했다. 장로에게 돌아가보자";
                QuestManager.Instance.NextQuest();
                QuestManager.Instance.killCount = 0;
            }
        }
        else if (id == 40 && index == 1)
        {
            qeustName.text = "장로가 자리비운 시간";
            qeustDetail.text = "장로회관에 가루와 관련된 물건이 있는지 한 번 둘러보자";
        }
        else if (id == 50 && index == 0)
        {
            qeustName.text = "마법이 담긴 함정 상자";
            qeustDetail.text = "상자를 열었더니 이상한 공간으로 빨려들어왔다. 퍼즐을 풀어 탈출하자";
        }
        else if (id == 60 && index == 0)
        {
            qeustName.text = "확실해진 재앙의 원인";
            qeustDetail.text = "유일하게 남은 주민에게 이 사실을 알리자";
        }
        else if (id == 60 && index == 1)
        {
            qeustName.text = "장로를 찾아라";
            qeustDetail.text = "사실을 알렸으니 이제 마을 회관에서 상자를 회수하고 장로를 기다리자.";
        }
        else if (id == 70 && index == 0)
        {
            qeustName.text = "장로를 제거하라";
            qeustDetail.text = "마을 근처....옛 놀이터였던 땅굴 입구를 찾아 들어가자.";
            if (QuestManager.Instance.bosskillCount == 1)
            {
                QuestManager.Instance.NextQuest();
                QuestManager.Instance.bosskillCount = 0;
                qeustName.text = "재앙 제거 완료";
                qeustDetail.text = "영생을 찾아 몬스터가 되어버린 장로를 제거했다....\n다시 마을로 돌아가자.";
            }
        }
        else if (id == 90 && index == 0)
        {
            qeustName.text = "다시 찾아온 평화";
            qeustDetail.text = "평화로운 마을을 다시 재건하자.";
        }

    }
    

    private void Start()
    {
        Close();
    }

    void Close()
    {
        gameObject.SetActive(false);
    }
}

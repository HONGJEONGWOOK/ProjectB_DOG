using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestPanel : MonoBehaviour
{

    TextMeshProUGUI qeustName;
    TextMeshProUGUI qeustDetail;

    private void Awake()
    {
        qeustName = transform.Find("QuestName").GetComponent<TextMeshProUGUI>();
        qeustDetail = transform.Find("QuestDetail").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        Panel();
    }

    void Panel()
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
            qeustDetail.text = "다른 생존자는 없을까? 마을과 집을 둘러보자!";
        }
        else if (id == 20 && index == 0)
        {
            qeustName.text = "장로에게 다시 가보자";
            qeustDetail.text = "재앙의 원인을 찾아야 한다. 장로에게 다시 돌아가자.";
        }
        else if (id == 20 && index == 1)
        {
            qeustName.text = "고블린이 원인?";
            qeustDetail.text = "재앙의 원인은 최근 늘어난 몬스터들일까? 주민의 의견도 물어보자.";

        }
        else if (id == 30 && index == 0)
        {
            qeustName.text = "고블린 5마리 처치!";
            qeustDetail.text = $"고블린 킬수 : {QuestManager.Instance.GoblinQuestCount}";
            if (QuestManager.Instance.GoblinQuestCount >= 5)
            {
                QuestManager.Instance.NextQuest();

                // 30 and 1에서 40 and 1로 넘어가도록
            }
        }
        else if (id == 40 && index == 0)
        {
            qeustName.text = "고블린 5마리 처치 완료";
            qeustDetail.text = "고블린 5마리를 처치했다. 장로에게 말하러 가자.";
        }

    }
}

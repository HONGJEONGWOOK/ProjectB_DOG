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
            qeustName.text = "누가 진실을 말하는 것일까";
            qeustDetail.text = "생존 주민이 말한데로 장로는 나에게 정체모를 액체를 주었다. 주민에게 다시 가보자";

        }
        else if (id == 30 && index == 0)
        {
            qeustName.text = "장로가 말해준 장소를 순찰하며 고블린을 처치하자";
            qeustDetail.text = $"고블린 킬수 : {QuestManager.Instance.GoblinQuestCount}";
            if (QuestManager.Instance.GoblinQuestCount >= 5)
            {
                QuestManager.Instance.NextQuest();

                // 30 and 1에서 40 and 1로 넘어가도록
            }
        }
        else if (id == 40 && index == 0)
        {
            qeustName.text = "순찰 완료";
            qeustDetail.text = "고블린도 적당히 처치하며 순찰했으나 별다른 원인이 안 보인다. 다시 장로에게 돌아가보자";
        }

    }
}

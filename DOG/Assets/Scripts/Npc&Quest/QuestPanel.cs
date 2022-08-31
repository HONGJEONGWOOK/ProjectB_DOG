using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Build.Content;

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
        Close();
    }

    void Close()
    {
        gameObject.SetActive(false);
    }

    void Open()
    {
        gameObject.SetActive(true);
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
            qeustName.text = "생존자에게 다시 돌아가보자";
            qeustDetail.text = "주민의 말처럼 장로에게 약물을 받았다. 우선 주민에게 상황을 말해주자.";

        }
        else if (id == 30 && index == 0)
        {
            qeustName.text = "마을 순찰 및 고블린 처치!";
            qeustDetail.text = $"빈 마을을 노리는 고블린을 처치하고 순찰하자!\n고블린 킬수 : {QuestManager.Instance.killcount}";
            if (QuestManager.Instance.killcount >= 5)
            {
                QuestManager.Instance.NextQuest();

                // 30 and 1에서 40 and 0로 넘어가도록
            }
        }
        else if (id == 40 && index == 0)
        {
            qeustName.text = "마을 순찰 및 고블린 처치 완료";
            qeustDetail.text = "고블린 5마리를 처치하며 순찰을 완료했다. 장로에게 돌아가보자";
        }
        else if (id == 40 && index == 1)
        {
            Invoke("ControlObject", 0.5f);
            qeustName.text = "장로가 자리비운 시간";
            qeustDetail.text = "장로회관에 가루와 관련된 물건이 있는지 한 번 둘러보자";
        }
        else if (id == 50 && index == 0)
        {
            qeustName.text = "마법이 담긴 함정 상자";
            qeustDetail.text = "상자를 열었더니 이상한 공간으로 빨려들어왔다. 탈출 방법을 찾아 탈출하자";
            LoadingSceneManager.LoadScene(5);
            //EnterPuzzleRoom.enterPuzzleRoom();
        }
        else if (id == 60 && index == 0)
        {
            qeustName.text = "확실해진 재앙의 원인";
            qeustDetail.text = "유일하게 남은 주민에게 이 사실을 알리자";
        }
        else if (id == 60 && index == 1)
        {
            qeustName.text = "장로를 찾아라";
            qeustDetail.text = "사실을 알렸으니 이제 마을 회관에서 장로를 기다려 주민들을 되찾자.";
        }
        else if (id == 70 && index == 0)
        {
            qeustName.text = "장로를 제거하라";
            qeustDetail.text = "마을 근처....옛 놀이터였던 땅굴 입구를 찾아 들어가자.";
        }
    }

    void ControlObject()
    {
        GameObject obj = GameObject.Find("VillageElder");
        obj.SetActive(false);
    }
}

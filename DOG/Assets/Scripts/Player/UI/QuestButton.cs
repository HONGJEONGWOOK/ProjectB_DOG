using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestButton : MonoBehaviour
{

    GameObject questPanel;

    void Awake()
    {
        questPanel = GameObject.Find("QuestPanel");
    }

    public void QuestOn()
    {
        questPanel.SetActive(true);
    }

    public void QuestOff()
    {
        questPanel.SetActive(false);
    }
}
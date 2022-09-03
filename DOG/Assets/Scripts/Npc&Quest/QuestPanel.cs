using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Build.Content;

public class QuestPanel : MonoBehaviour
{
    public TextMeshProUGUI qeustName;
    public TextMeshProUGUI qeustDetail;

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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatusWindowButton : MonoBehaviour
{

    GameObject StatusWindow;

    private void Awake()
    {
        StatusWindow = GameObject.Find("StatusWindow");
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
    public void StatusWindowOn()
    {
        StatusWindow.SetActive(true);
    }

    public void StatusWindowOff()
    {
        StatusWindow.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatusWindowButton : MonoBehaviour
{

    GameObject StatusWindow;

    void Start()
    {
        StatusWindow = GameObject.Find("StatusWindow");
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{

    GameObject Menu;

    bool isopen;

    void Awake()
    {
        isopen = false;
        Menu = GameObject.Find("MenuBackground");
    }

    private void Update()
    {
        
    }

    public void MenuOn()
    {
        
            Menu.SetActive(true);
            SoundManager.Inst.PlaySound(SoundID.click, true);
    }

    public void MenuOff()
    {
        Menu.SetActive(false);
        SoundManager.Inst.PlaySound(SoundID.click, true);
    }
}
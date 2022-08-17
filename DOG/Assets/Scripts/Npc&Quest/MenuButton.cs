using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{

    GameObject Menu;

    void Awake()
    {
        Menu = GameObject.Find("MenuBackground");
    }

    public void MenuOn()
    {
        Menu.SetActive(true);
    }

    public void MenuOff()
    {
        Menu.SetActive(false);
    }
}

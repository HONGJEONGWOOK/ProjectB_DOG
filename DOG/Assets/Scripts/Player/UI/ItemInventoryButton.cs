using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemInventoryButton : MonoBehaviour
{

    GameObject ItemInventory;

    void Start()
    {
        ItemInventory = GameObject.Find("ItemInventory");
    }

    void Update()
    {

    }
    public void ItemInventoryOn()
    {
        ItemInventory.SetActive(true);
    }

    public void ItemInventoryOff()
    {
        ItemInventory.SetActive(false);
    }
}

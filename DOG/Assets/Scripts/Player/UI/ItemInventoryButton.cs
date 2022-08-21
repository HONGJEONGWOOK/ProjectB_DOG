using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemInventoryButton : MonoBehaviour
{

    GameObject ItemInventory;

    private void Awake()
    {
        ItemInventory = GameObject.Find("ItemInventory");
    }

    void Start()
    {
        
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

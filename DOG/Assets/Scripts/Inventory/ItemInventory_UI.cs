using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory_UI : MonoBehaviour
{
    Inventory inven; // 찾음

    ItemSlot_UI[] slotUI; // 찾음


    private void Awake()
    {
    }

    public void InitializeInven(Inventory newInven)
    {
        inven = newInven;

        slotUI = GetComponentsInChildren<ItemSlot_UI>();

        for (int i = 0; i < slotUI.Length; i++)
        {
            slotUI[i].InitializeSlotUI((uint)i, inven[i]);
        }
    }
}

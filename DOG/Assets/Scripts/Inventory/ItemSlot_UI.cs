using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot_UI : MonoBehaviour
{
    ItemSlot slot;

    uint slotID;

    // 표시할 것
    Image icon;


    public void InitializeSlotUI(uint id, ItemSlot target)
    {
        slotID = id;
        slot = target;

        icon = transform.GetChild(0).GetComponent<Image>();

        slot.OnItemUpdate += Refresh;
    }

    public void Refresh()
    {
        if (slot.SlotData != null)
        {
            icon.sprite = slot.SlotData.icon;
            icon.color = Color.white;
        }
        else
        {
            icon.sprite = null;
            icon.color = Color.clear;
        }
        
    }
}

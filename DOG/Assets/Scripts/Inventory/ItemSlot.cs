using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot
{
    ItemData data;

    uint count = 0;
    // ############### Delegate
    public System.Action OnItemUpdate;

    // ############### Property
    public ItemData SlotData
    {
        get => data;
        set
        {
            data = value;
            OnItemUpdate?.Invoke();
        }
    }

    public uint Count
    {
        get => count;
        set
        {
            count = value;
            OnItemUpdate?.Invoke();
        }
    }

    public void AssignItem(ItemData itemData, uint num = 1)
    {
        SlotData = itemData;
        Count += num;
        //Debug.Log($"아이템 추가 : {data.name}");
    }

    public void ClearSlot()
    {
        SlotData = null;
        //Debug.Log("아이템 제거");
    }

    public void IncreaseItem(uint num = 1)
    {
        Count += num;
    }

    public void DecreaseSlotItem(uint count = 1)
    {
        int newCount = (int)Count - (int)count;
        if (newCount < 1)
        {
            ClearSlot();
        }
        else
        {
            Count = (uint)newCount;
        }
    }

    public void UseSlotItem(GameObject target = null)
    {
        IUsable usable = SlotData as IUsable;
        if (usable != null)
        {
            usable.Use(target);
            DecreaseSlotItem();
        }
    }

}

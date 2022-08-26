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
        Debug.Log($"아이템 추가 : {data.name}");
    }

    public void ClearSlot()
    {
        data = null;
        Debug.Log("아이템 제거");
    }

    public void IncreaseItem(uint num = 1)
    {
        Count += num;
    }
}

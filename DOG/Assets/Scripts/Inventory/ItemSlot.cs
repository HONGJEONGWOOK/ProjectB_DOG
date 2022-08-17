using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot
{
    ItemData data;

    // ############### Property
    public ItemData SlotData => data;
    
    public void AssignItem(ItemData itemData, uint num = 1)
    {
        data = itemData;
        data.count += num;
        Debug.Log($"아이템 추가 : {data.name}");
    }

    public void ClearSlot()
    {
        data = null;
        Debug.Log("아이템 제거");
    }

    public void IncreaseItem(uint num = 1)
    {
        data.count += num;
    }
}

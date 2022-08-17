using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    ItemSlot[] slots;
    // Slot 인덱서
    public ItemSlot this[int index] { get => slots[index]; }

    public const uint DEFAULT_SIZE = 6;
    public int SlotCount => slots.Length;

    // Inventory 생성자 
    public Inventory(uint size = DEFAULT_SIZE)
    {
        slots = new ItemSlot[size];
        for (int i = 0; i < size; i++)
        {
            slots[i] = new ItemSlot();
        }
    }

    // ########## Inventory 함수
    public void AddItem(ItemData data)
    {
        ItemSlot slot;
        
        slot = FindSameSlot(data);
        if (slot != null)
        {// 같은 아이템이 있음.
            if( slot.Count < slot.SlotData.maxCount)
            slot.IncreaseItem();
            return;
        }

        slot = FindSlot();
        if (slot != null)
        {// 새로운 아이템 추가
            slot.AssignItem(data);
        }
        else
        {// 추가 실패
            Debug.Log("아이템 추가 실패");
        }
    }

    public void AddItem(ItemID id)
    {
        AddItem(GameManager.Inst.ItemData[id]);
    }


    public void RemoveItem(uint slotIndex, uint num = 1)
    {
        if (slotIndex < slots.Length)
            slots[slotIndex].Count -= num;
    }


    ItemSlot FindSlot()
    {
        ItemSlot emptySlot = null;

        foreach (ItemSlot slot in slots)
        {
            if (slot.SlotData == null)
            {
                emptySlot = slot;
                break;
            }
        }
        return emptySlot;
    }

    ItemSlot FindSameSlot(ItemData data)
    {
        ItemSlot sameSlot = null;

        foreach (ItemSlot slot in slots)
        {
            if (slot.SlotData == data && slot.Count < slot.SlotData.maxCount)
            {// 중복되는 아이템이 있다.
                sameSlot = slot;
                break;
            }
        }
        return sameSlot;
    }

    public void TestInventory()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].SlotData != null)
                Debug.Log($"{(i)}번 슬롯 : {slots[i].Count} 개");
        }

        foreach (ItemSlot slot in slots)
        {
            if (slot.SlotData != null)
                slot.Count = 0;
        }
    }
}

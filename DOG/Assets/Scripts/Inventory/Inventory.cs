using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    ItemSlot[] slots;
    ItemSlot movingSlot;
    ItemSlot oldSlot;

    // Slot 인덱서
    public ItemSlot this[int index] { get => slots[index]; }

    public ItemSlot MovingSlot => movingSlot;
    public ItemSlot OldSlot => oldSlot;

    public const uint DEFAULT_SIZE = 13;
    public int SlotCount => slots.Length;

    public const uint MOVINGSLOT_ID = 100;

    // Inventory 생성자 
    public Inventory(uint size = DEFAULT_SIZE)
    {
        slots = new ItemSlot[size];
        for (int i = 0; i < size; i++)
        {
            slots[i] = new ItemSlot();
        }
        movingSlot = new ItemSlot();
        oldSlot = new();
    }

    // ####################### Inventory 함수 ############################
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

    public void AddItem(uint index)
    {
        ItemSlot slot = slots[index];

        slot.AssignItem(slot.SlotData);
    }

    public void AddItem(ItemID id, uint index)
    {
        ItemSlot slot = slots[index];

        slot.AssignItem(GameManager.Inst.ItemData[id]);
    }


    public void RemoveItem(uint slotIndex, uint num = 1)
    {
        if (slotIndex < slots.Length)
            slots[slotIndex].Count -= num;
    }

    public void MoveItem(uint current, uint destination)
    {
        ItemSlot currentSlot;
        ItemSlot destinationSlot;

        if (current == MOVINGSLOT_ID)
        {
            currentSlot = movingSlot;
        }
        else
        {
            currentSlot = slots[current];
        }

        if (destination == MOVINGSLOT_ID)
        {
            destinationSlot = movingSlot;
        }
        else
        {
            destinationSlot = slots[destination];
        }

        if (currentSlot.SlotData == destinationSlot.SlotData)
        {// 같은 아이템
            if (currentSlot.Count + destinationSlot.Count < destinationSlot.SlotData.maxCount + 1)
            {// 넘치지 않을 때 (Merge)
                destinationSlot.Count += currentSlot.Count;
                currentSlot.SlotData = null;

                Debug.Log($"{current}를 {destination}으로 옮깁니다.");
            }
            else
            {// 넘칠 때
                uint remainder = currentSlot.Count + destinationSlot.Count - destinationSlot.SlotData.maxCount;
                destinationSlot.Count = destinationSlot.SlotData.maxCount;
                movingSlot.Count = remainder;
                Debug.Log($"{remainder}개가 넘쳤습니다.");
            }
        }
        else
        {// 다른 아이템
            ItemSlot tempSlot;
            tempSlot = destinationSlot;

            destinationSlot.SlotData = currentSlot.SlotData;
            destinationSlot.Count = currentSlot.Count;

            currentSlot.SlotData = tempSlot.SlotData;
            currentSlot.Count = tempSlot.Count;

            tempSlot = null;
            Debug.Log($"{current}를 {destination}으로 옮깁니다.");
        }
    }

    public void MoveToOldSlot(uint current)
    {
        oldSlot.AssignItem(slots[current].SlotData, slots[current].Count);
    }

    public void ReturnOldSlot(uint returningSlot)
    {
        slots[returningSlot].Count = 0;
        slots[returningSlot].AssignItem(oldSlot.SlotData, oldSlot.Count);
        oldSlot.SlotData = null;
        oldSlot.Count = 0;
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
    }
}

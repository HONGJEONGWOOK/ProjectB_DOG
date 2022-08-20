using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ItemInventory_UI : MonoBehaviour, IPointerClickHandler
{
    PlayerInputActions Actions;

    Inventory inven; // 찾음

    ItemSlot_UI[] slotUI; // 찾음
    MovingSlot_UI movingSlotUI; // 찾음

    uint oldSlotID = 50;

    private void Awake()
    {
        Actions = new();
    }

    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void InitializeInven(Inventory newInven)
    {
        inven = newInven;

        slotUI = transform.GetChild(2).GetComponentsInChildren<ItemSlot_UI>();
        movingSlotUI = transform.GetChild(3).GetComponent<MovingSlot_UI>();

        for (int i = 0; i < slotUI.Length; i++)
        {
            slotUI[i].InitializeSlotUI((uint)i, inven[i]);
        }
        movingSlotUI.InitializeSlotUI(Inventory.MOVINGSLOT_ID, inven.MovingSlot);

        movingSlotUI.ShowMovingSlotUI(false);
    }

    // moving slot에 아무것도 없을 떄 클릭하면 moving slot으로 옮겨지고,
    // 뭔가 있을 때는 moving slot에서 아이템 옮기기
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            GameObject clickedObject = eventData.pointerCurrentRaycast.gameObject;
            Debug.Log(clickedObject.name);
            if (clickedObject != null)
            {
                ItemSlot_UI clickedSlot = clickedObject.transform.parent.GetComponent<ItemSlot_UI>();
                if (clickedSlot != null)
                {
                    //if (clickedSlot.Slot.SlotData != null && movingSlotUI.Slot.SlotData == null)
                    //{// moving slot에 아무것도 없을 때
                    //    oldSlotID = clickedSlot.SlotID; // moving slot으로 간 원래 slotID
                    //    inven.MoveToMovingSlot(clickedSlot.SlotID);

                    //    // Show MovingSlot
                    //    movingSlotUI.gameObject.SetActive(true);
                    //    Debug.Log(inven.MovingSlot.SlotData.name);
                    //}
                    //else if (clickedSlot.Slot.SlotData != null && movingSlotUI.Slot.SlotData != null)
                    //{// 이미 아이템이 있는 슬롯에 아이템을 놓을 때
                    //    inven.MoveItem(clickedSlot.SlotID, oldSlotID);
                    //    inven.MovingSlotToSlot(clickedSlot.SlotID);  //inven.MoveItem(Inventory.MOVINGSLOT_ID, clickedSlot.SlotID);
                    //    movingSlotUI.Slot.SlotData = null;

                    //    // Hide MovingSlot
                    //    movingSlotUI.gameObject.SetActive(false);
                    //    Debug.Log($"{clickedSlot.SlotID} <-> {oldSlotID} ");
                    //}
                    //else  //ClickedSlot이 있을 때
                    //{// moving slot에 아이템이 있을 때 
                    //    inven.MovingSlotToSlot(clickedSlot.SlotID);

                    //    // Hide MovingSlot
                    //    movingSlotUI.gameObject.SetActive(false);
                    //    Debug.Log("Moving Slot --> Clicked Slot");
                    //}
                    if (movingSlotUI.Slot.SlotData == null)
                    {// 옮기는 물건이 없으면
                        // 스왑용 oldSlot에 저장
                        if (clickedSlot.Slot.SlotData == null)
                            return;
                        oldSlotID = clickedSlot.SlotID;

                        inven.MoveItem(clickedSlot.SlotID, Inventory.MOVINGSLOT_ID);
                        clickedSlot.Slot.SlotData = null;
                        movingSlotUI.ShowMovingSlotUI(true);
                    }
                    else
                    {// 옮기는 물건이 있으면
                        if (clickedSlot.Slot.SlotData != movingSlotUI.Slot.SlotData)
                        {// 다른 아이템일 때 ( 빈 아이템일경우 그냥 놓기 )
                            if (clickedSlot.Slot.SlotData == null)
                            {
                                inven.MoveItem(Inventory.MOVINGSLOT_ID, clickedSlot.SlotID);
                            }
                            else
                            {// 원래 아이템이랑 클릭한 아이템이랑 스왑
                                inven.MoveToOldSlot(clickedSlot.SlotID);
                                inven.MoveItem(Inventory.MOVINGSLOT_ID, clickedSlot.SlotID);
                                inven.ReturnOldSlot(oldSlotID);  //old
                                oldSlotID = 50;
                            }
                            movingSlotUI.Slot.SlotData = null;
                            
                        }
                        else if(clickedSlot.Slot.SlotData == movingSlotUI.Slot.SlotData)
                        {// 같은 아이템일 때 (클릭한 슬롯에 채우고, 나머지를 무빙슬롯에 저장)
                            inven.MoveItem(Inventory.MOVINGSLOT_ID, clickedSlot.SlotID);
                        }
                        else
                        {// 잘못된 선택. 원래 자리로 돌아가기
                            inven.MoveItem(Inventory.MOVINGSLOT_ID, oldSlotID);
                        }

                        if (movingSlotUI.Slot.SlotData == null)
                        {
                            movingSlotUI.ShowMovingSlotUI(false);
                        }
                    }
                }
                else
                {// 인벤토리 밖에 버릴 때 클릭한게 슬롯이 아닐 때
                    if (movingSlotUI.Slot.SlotData != null)
                    {
                        Debug.Log("아이템 버림");
                        for (int i = 0; i < movingSlotUI.Slot.Count; i++)
                        {
                            uint droppingItemID = movingSlotUI.Slot.SlotData.id;
                            GameObject obj = ItemManager.GetPooledItem(ItemManager.Inst.PooledItems[droppingItemID]);
                            obj.transform.position = eventData.pointerCurrentRaycast.worldPosition + (Vector3)UnityEngine.Random.insideUnitCircle;
                            obj.SetActive(true);
                        }
                        movingSlotUI.Slot.SlotData = null;
                        movingSlotUI.ShowMovingSlotUI(false);
                    }
                }
            }
        }
    }
}

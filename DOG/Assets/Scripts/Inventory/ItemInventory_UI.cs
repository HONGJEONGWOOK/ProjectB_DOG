using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemInventory_UI : MonoBehaviour, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    DetailInfoUI detail;
    public DetailInfoUI Detail => detail;

    PlayerInputActions Actions;
    Animator anim;

    Inventory inven; // 찾음

    ItemSlot_UI[] slotUI; // 찾음
    MovingSlot_UI movingSlotUI; // 찾음

    uint oldSlotID = 50;

    // Main Inventory
    Transform mainInven;
    Transform subInven;
    Button closeButton;
    Button invenMenuButton;
    CanvasGroup canvasGroup;
    bool isOpen = false;

    public Inventory Inven => inven;

    Vector2 dragOffset;


    public bool IsOpen
    {
        get
        {
            if(isOpen) isOpen = false; else isOpen = true;
            return isOpen;
        }
    }

    private void Awake()
    {
        Actions = new();
        mainInven = transform.GetChild(0);
        canvasGroup = mainInven.GetChild(0).GetComponent<CanvasGroup>();
        closeButton = mainInven.GetChild(0).GetChild(2).GetComponent<Button>();

        detail = GetComponentInChildren<DetailInfoUI>();

        invenMenuButton = transform.parent.GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>();
        //Debug.Log(invenMenuButton.name);
        anim = GetComponent<Animator>();

        closeButton.onClick.AddListener(()=> ShowInventory(IsOpen));
        invenMenuButton.onClick.AddListener(() => ShowInventory(IsOpen));
    }

    private void OnEnable()
    {
        Actions.UI.Enable();
        Actions.UI.InventoryButton.performed += OnInvenButtonInput;
    }

    private void Start()
    {
        ShowInventory(isOpen);
    }

    public void InitializeInven(Inventory newInven)
    {
        inven = newInven;

        slotUI = mainInven.GetComponentsInChildren<ItemSlot_UI>();
        movingSlotUI = GetComponentInChildren<MovingSlot_UI>();

        for (int i = 0; i < slotUI.Length; i++)
        {
            slotUI[i].InitializeSlotUI((uint)i, inven[i]);
        }
        movingSlotUI.InitializeMovingSlotUI(Inventory.MOVINGSLOT_ID, inven.MovingSlot);

        movingSlotUI.ShowMovingSlotUI(false);
    }

    private void ShowInventory(bool isShow)
    {
        canvasGroup.alpha = isShow ? 1.0f: 0.0f;
        canvasGroup.blocksRaycasts = isShow;
        SoundManager.Inst.PlaySound(SoundID.windowOpen, 1f, true);
    }

    private void OnInvenButtonInput(InputAction.CallbackContext _)
    {
        ShowInventory(IsOpen);
    }

    // moving slot에 아무것도 없을 떄 클릭하면 moving slot으로 옮겨지고,
    // 뭔가 있을 때는 moving slot에서 아이템 옮기기
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            GameObject clickedObject = eventData.pointerCurrentRaycast.gameObject;
            if (clickedObject != null)
            {
                ItemSlot_UI clickedSlot = clickedObject.transform.parent.GetComponent<ItemSlot_UI>();
                if (clickedSlot != null)
                {
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
                        movingSlotUI.Slot.Count = 0;
                        movingSlotUI.ShowMovingSlotUI(false);
                    }
                }
            }

            SoundManager.Inst.PlaySound(SoundID.click, 0.8f ,true);
        }
        else if(eventData.button == PointerEventData.InputButton.Right)
        {
            GameObject clickedObject = eventData.pointerCurrentRaycast.gameObject;
            if (clickedObject != null)
            {
                ItemSlot_UI clickedSlot = clickedObject.transform.parent.GetComponent<ItemSlot_UI>();
                if (clickedSlot != null)
                {

                    clickedSlot.Slot.UseSlotItem(GameManager.Inst.MainPlayer.gameObject);
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragOffset = (Vector2)mainInven.GetChild(0).transform.position - eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        mainInven.GetChild(0).transform.position = eventData.position + dragOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //if(Camera.main.WorldToScreenPoint(mainInven.GetChild(0).transform.position) > Screen.saf)
    }
}

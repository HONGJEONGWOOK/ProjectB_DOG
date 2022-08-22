using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemSlot_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    ItemInventory_UI invenUI;    // 인벤토리 UI
    DetailInfoUI detailUI;  // 상세 정보창

    protected ItemSlot slot;

    uint slotID;

    // 표시할 것
    Image icon;
    TextMeshProUGUI countText;

    // 마우스 오버 애니메이션
    RectTransform iconRect;
    Vector2 originalSize;

    // Slot 프로퍼티
    public ItemSlot Slot => slot;
    public uint SlotID => slotID;

    public void InitializeSlotUI(uint id, ItemSlot target)
    {
        slotID = id;
        slot = target;

        icon = transform.GetChild(0).GetComponent<Image>();
        countText = GetComponentInChildren<TextMeshProUGUI>();
        iconRect = icon.GetComponent<RectTransform>();
        originalSize = iconRect.sizeDelta;

        slot.OnItemUpdate += Refresh;

        invenUI = GameManager.Inst.InvenUI; // 미리 찾아놓기
        detailUI = invenUI.Detail;
    }

    public void Refresh()
    {
        if (slot.SlotData != null)
        {
            icon.sprite = slot.SlotData.icon;
            icon.color = Color.white;
            countText.text = slot.Count.ToString();
        }
        else
        {
            icon.sprite = null;
            icon.color = Color.clear;
            countText.text = "0";
        }
    }

    // 마우스 접근 시 행동
    public void OnPointerEnter(PointerEventData eventData)
    {
        iconRect.sizeDelta *= 1.2f;
        if (slot.SlotData != null)
        {
            detailUI.Open(slot.SlotData);
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        iconRect.sizeDelta = originalSize;
        detailUI.Close();
    }
}

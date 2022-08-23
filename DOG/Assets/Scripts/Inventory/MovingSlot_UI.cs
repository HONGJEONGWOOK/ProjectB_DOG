using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class MovingSlot_UI : ItemSlot_UI
{
    CanvasGroup group;

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        transform.position = Mouse.current.position.ReadValue();
    }

    public void ShowMovingSlotUI(bool show)
    {
        if (show)
        {
            group.alpha = 1f;
        }
        else
        {
            group.alpha = 0f;
        }
    }

    public void InitializeMovingSlotUI(uint id, ItemSlot target)
    {
        slotID = id;
        slot = target;

        icon = transform.GetChild(0).GetComponent<Image>();
        countText = GetComponentInChildren<TextMeshProUGUI>();
        iconRect = icon.GetComponent<RectTransform>();
        originalSize = iconRect.sizeDelta;

        slot.OnItemUpdate += Refresh;

        invenUI = GameManager.Inst.InvenUI; // 미리 찾아놓기
    }
}


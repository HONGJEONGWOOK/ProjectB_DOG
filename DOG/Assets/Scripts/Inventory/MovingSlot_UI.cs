using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MovingSlot_UI : ItemSlot_UI, IPointerClickHandler
{
    PlayerInputActions actions;
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (!eventData.pointerCurrentRaycast.gameObject.CompareTag("ItemInven"))
            {

            }
        }
    }
}


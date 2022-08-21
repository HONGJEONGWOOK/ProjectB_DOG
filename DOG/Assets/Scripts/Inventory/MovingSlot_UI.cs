using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovingSlot_UI : ItemSlot_UI
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
}


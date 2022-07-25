using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_TakeDamage : MonoBehaviour
{
    public Goblin goblin = null;

    private void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            goblin.TakeDamage(30f);
        }
    }
}

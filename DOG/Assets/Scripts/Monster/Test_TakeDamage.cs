using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_TakeDamage : MonoBehaviour
{
    public Monsters testMonster = null;

    private void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            testMonster.TakeDamage(30f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_TakeDamage : MonoBehaviour
{
    public Boss monster = null;

    private void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            monster.TakeDamage(300f);
        }
    }

}

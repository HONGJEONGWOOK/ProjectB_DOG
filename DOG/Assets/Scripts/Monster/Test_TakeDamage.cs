using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_TakeDamage : MonoBehaviour
{
    Goblin monster = null;

    private void Start()
    {
        monster = FindObjectOfType<Goblin>();
    }
    private void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            monster.TakeDamage(3f);
        }
    }

}

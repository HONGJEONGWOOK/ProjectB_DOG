using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class TestPlayer : MonoBehaviour
{
    public Player_Hero Player_Hero;
    private void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            Player_Hero.TakeDamage(30);
        }
    }
}


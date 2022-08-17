using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_TakeDamage : MonoBehaviour
{
    [SerializeField] Boss monster = null;
    IBattle target;


    private void Start()
    {
        monster = FindObjectOfType<Boss>();
            //monster.GetComponent<IBattle>();
    }
    private void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            monster.TakeDamage(300f);
        }
    }

}

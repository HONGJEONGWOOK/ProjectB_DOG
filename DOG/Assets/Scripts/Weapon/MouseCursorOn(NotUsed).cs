using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseCursorOn : MonoBehaviour
{
    Vector2 MousePosition;
    Camera Camera;

    //public GameObject[] slots;



    private void Awake()
    {
        Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        //slots = GameObject.FindWithTag("WeaponSelect");
    }


    private void Update()
    {
        //Vector3 mousePosition = Mouse.current.position.ReadValue();

        MousePosition = Input.mousePosition;
        MousePosition = Camera.ScreenToWorldPoint(MousePosition);


        //transform.position

        

    }
}

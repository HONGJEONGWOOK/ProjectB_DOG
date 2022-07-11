using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    

    public static Weapon instance;

    Animator anim;
    BoxCollider2D box;

    private void Awake()
    {
        instance = this;

        anim = GetComponent<Animator>();
        box = GetComponentInChildren<BoxCollider2D>();
    }


    public void Swing()
    {
        box.enabled = true;
        
        anim.SetTrigger("Attack");

        Invoke("OffBox", 1.0f);
        
    }

    public void OffBox()
    {
        box.enabled = false;
    }



}

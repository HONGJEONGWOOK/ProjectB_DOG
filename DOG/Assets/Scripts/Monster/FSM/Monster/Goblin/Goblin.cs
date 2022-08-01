using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Monsters, IHealth, IBattle
{
    private GameObject hpUI = null;

    protected override void Awake()
    {
        base.Awake();

        hpUI = transform.GetComponentInChildren<HP_Bar_Monster>().gameObject;
        hpUI.SetActive(false);
    }

    private void Start()
    {
        
    }
}

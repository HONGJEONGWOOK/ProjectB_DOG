using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster.Enums;

public class Goblin : Monsters, IHealth, IBattle
{
    private HP_Bar hpUI = null;

    protected override void Awake()
    {
        base.Awake();

        hpUI = GetComponentInChildren<HP_Bar>();
        hpUI.gameObject.SetActive(false);
    }
}

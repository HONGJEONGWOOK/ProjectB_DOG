using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    float hp = 100.0f;
    float maxHP = 100.0f;


    public float HP
    {
        get => hp;
        set
        {
            if (hp != value)
            {
                hp = value;
                onHealthChange?.Invoke();
            }
        }
    }

    public float MaxHP
    {
        get => maxHP;
    }

    public System.Action onHealthChange { get; set; }
}

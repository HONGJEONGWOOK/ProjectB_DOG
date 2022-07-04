using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public static PlayerStat inst;


    public int character_Lv;
    public float[] needExp;

    public float currentEXP;

    public int atk;
    public int def;


    private void Start()
    {
        inst = this;
    }


}

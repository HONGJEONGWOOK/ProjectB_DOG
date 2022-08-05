using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get=>instance;}

    // -----------------Boss Map Controller
    private BossTextController bossText;
    public BossTextController BossText => bossText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            instance.Initailize();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            if (instance == this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void Initailize()
    {
    }
}

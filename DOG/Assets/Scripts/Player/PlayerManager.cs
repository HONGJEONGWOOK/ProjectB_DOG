using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    static PlayerManager instance = null; 

    Player_Hero player;
    
    public Player_Hero MainPlayer
    {
        get => player;
    }

    public static PlayerManager Inst
    {
        get => instance;
    }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            instance.Initialize();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            if (instance != null)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void Initialize()
    {
        player = FindObjectOfType<Player_Hero>();
    }
}

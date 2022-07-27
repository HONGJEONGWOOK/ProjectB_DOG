using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    private static MonsterManager instance = null;
    public static MonsterManager Inst { get => instance; }

    Goblin monster_Goblin = null;
    Monster_Bow monster_Bow = null;
    Monster_Bow_V2 monster_Bow_V2 = null;
    Monster_Bow_V3 monster_Bow_V3 = null;


    public Goblin Monster_Goblin { get => monster_Goblin; }
    public Monster_Bow Monster_Bow { get => monster_Bow; }
    public Monster_Bow_V2 Monster_Bow_V2 => monster_Bow_V2;
    public Monster_Bow_V3 Monster_Bow_V3 => monster_Bow_V3;

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
        //monster_Goblin = FindObjectOfType<Goblin>().GetComponent<Goblin>();
        monster_Bow = FindObjectOfType<Monster_Bow>().GetComponent<Monster_Bow>();
        monster_Bow_V2 = FindObjectOfType<Monster_Bow_V2>().GetComponent<Monster_Bow_V2>();
        monster_Bow_V3 = FindObjectOfType<Monster_Bow_V3>().GetComponent<Monster_Bow_V3>();
    }
}

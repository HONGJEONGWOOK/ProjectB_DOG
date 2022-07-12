using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    private static MonsterManager instance = null;
    public static MonsterManager Inst { get => instance; }

    Monsters monster_Goblin = null;
    Monster_Bow monster_Bow = null;

    public Monsters goblinInst { get => monster_Goblin; }
    public Monster_Bow BowInst { get => monster_Bow; }

    private void Awake()
    {
        if (instance = null)
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
        monster_Goblin = FindObjectOfType<Monsters>().GetComponent<Monsters>();
        monster_Bow = FindObjectOfType<Monster_Bow>().GetComponent<Monster_Bow>();
    }
}
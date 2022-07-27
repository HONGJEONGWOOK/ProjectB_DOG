using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    private static MonsterManager instance = null;
    public static MonsterManager Inst { get => instance; }

    Monsters monster_Goblin = null;
    Monster_Bow monster_Bow = null;

<<<<<<< Updated upstream:DOG/Assets/Scripts/FSM/Monster/MonsterManager.cs
    public Monsters goblinInst { get => monster_Goblin; }
    public Monster_Bow BowInst { get => monster_Bow; }
=======

    public Goblin Monster_Goblin { get => monster_Goblin; }
    public Monster_Bow Monster_Bow { get => monster_Bow; }
>>>>>>> Stashed changes:DOG/Assets/Scripts/Monster/FSM/Monster/MonsterManager.cs

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
<<<<<<< Updated upstream:DOG/Assets/Scripts/FSM/Monster/MonsterManager.cs

        detectCollider = FindObjectOfType<MonsterManager>().GetComponent<CircleCollider2D>();
        target = FindObjectOfType<Player_Move>().GetComponent<Transform>();

=======
>>>>>>> Stashed changes:DOG/Assets/Scripts/Monster/FSM/Monster/MonsterManager.cs
    }

    private void Initialize()
    {
<<<<<<< Updated upstream:DOG/Assets/Scripts/FSM/Monster/MonsterManager.cs
        monster_Goblin = FindObjectOfType<Monsters>().GetComponent<Monsters>();
        monster_Bow = FindObjectOfType<Monster_Bow>().GetComponent<Monster_Bow>();
=======
        //monster_Goblin = FindObjectOfType<Goblin>().GetComponent<Goblin>();
        //monster_Bow = FindObjectOfType<Monster_Bow>().GetComponent<Monster_Bow>();
>>>>>>> Stashed changes:DOG/Assets/Scripts/Monster/FSM/Monster/MonsterManager.cs
    }
}

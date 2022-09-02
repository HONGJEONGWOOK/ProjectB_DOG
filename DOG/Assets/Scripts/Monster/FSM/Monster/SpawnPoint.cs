using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public bool hasMonster = false;
    public MonsterID monsterType;
    public GameObject monster;

    public SpawnPoint()
    {
        this.hasMonster = false;
        this.monsterType = MonsterID.GOBLIN;
        this.monster = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterCurrentState
{
    IDLE = 0,
    PATROL,
    TRACK,
    ATTACK,
    KNOCKBACK,
    DEAD
}

public enum MonsterID 
{
    GOBLIN = 0,
    TREANT,
    BOSS
}

public enum ItemID 
{
    HPPotion = 0,
    ManaPotion,
    GoblinsPPP,
    Arrows
}



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


public enum SoundID : byte
{
    BGM_Village = 0,
    BGM_Field,
    BGM_House,
    BGM_Dungeon,
    BGM_Boss,
    BGM_Minigame,

    //Player
    playerFootStep,
    playerGetHit,
    SwordSwing,
    
    // UI
    click,
    windowOpen,
    pointerOnSlot,
    weaponChange,
    potionUse,
    QuestComplete,

    //Goblin
    GoblinGetHit,
    GoblinAttack,
    GoblinDie,

    // -- Boss
    BossHit,
    BossBite,
    BossAttack1,
    BossDie,
    MeteorFly,
    MeteorExplosion,
    BossMove,

    // -- Treant
    ShootArrow,
    TreantGetHit,
    TreantDie,

    // -- Minigame
    Rock,
    Gate,
    ClipNumbers
}



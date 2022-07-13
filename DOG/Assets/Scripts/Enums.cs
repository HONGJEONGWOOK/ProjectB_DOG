using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster.Enums
{
    public enum MonsterCurrentState
    {
        IDLE = 0,
        PATROL,
        TRACK,
        ATTACK,
        HIT,
        DEAD
    }
}

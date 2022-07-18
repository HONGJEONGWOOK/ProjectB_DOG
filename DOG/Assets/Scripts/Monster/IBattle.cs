using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattle
{
    void Attack(IBattle target);

    void TakeDamage(float damage);

    float AttackPower { get; }
    float Defence { get; }
}

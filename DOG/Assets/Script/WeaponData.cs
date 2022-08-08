using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Scriptable Object/Item Data", order = 1)]

// 무기의 정보
public class WeaponData : ScriptableObject
{
    public uint id = 0;
    public WeaponType type;
    public Sprite itemIcon;
    public GameObject prefab;
    public int strength;
    public int dexterity;
    public int accuracy;
}

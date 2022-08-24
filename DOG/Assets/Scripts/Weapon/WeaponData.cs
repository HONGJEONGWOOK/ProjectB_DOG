using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Scriptable Object/Weapon Data", order = 1)]

// 무기의 정보
public class WeaponData : ScriptableObject
{
    public string weaponName;
    //public uint id = 0;
    public WeaponType type; // 0이 sword 1이 dagger 2가 bow
    //public uint weaponType = 0;  
    public Sprite weaponSprite;
    public GameObject prefab;
    //public int strength;
    //public int dexterity;
    //public int accuracy;
    public float attackPower = 0.0f;
    public float defencePower = 0.0f;
    public float criticalRate = 0.0f;
}


public enum WeaponType            // 무기 종류 enum
{
    Sword = 0,
    Dagger = 1,
    Bow = 2
}


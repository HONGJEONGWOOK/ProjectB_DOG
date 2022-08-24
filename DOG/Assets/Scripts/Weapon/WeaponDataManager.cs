using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 무기 데이터 매니저
public class WeaponDataManager : MonoBehaviour
{
    public WeaponData[] weaponDatas;

    public WeaponData this[uint i]
    {
        get => weaponDatas[i];
    }

    public WeaponData this[WeaponType type]
    {
        get => weaponDatas[(uint)type];
    }
}

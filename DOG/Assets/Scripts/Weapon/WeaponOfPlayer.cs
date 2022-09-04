using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player의 자식 Weapon에 들어가는 무기 코드
public class WeaponOfPlayer : MonoBehaviour
{
    WeaponData playerWeaponData;

    public WeaponData PlayerWeaponData          // 플레이어 무기데이터를 받아와서 설정
    {
        get => playerWeaponData;
        private set
        {
            if (playerWeaponData != value)
            {
                playerWeaponData = value;
            }
        }
    }
    uint defaultWeapon = 0;     // 초기 무기(검)
    public Vector3 weaponPosition = new Vector3(0.21f, 0.76f, 0);  // 무기의 위치


    private void Start()
    {
        AssignWeapon(GameManager.Inst.WeaponData[defaultWeapon]);
        //EquipWeapon(defaultWeapon);
    }


    public void AssignWeapon(WeaponData weaponData)
    {
        PlayerWeaponData = weaponData;
    }

    public void ChangeWeapon(uint id)
    {
        UnEquipWeapon();
        AssignWeapon(GameManager.Inst.WeaponData[id]);
        EquipWeapon(id);
        
    }


    public void EquipWeapon(uint id)
    {
        GameObject obj = Instantiate(PlayerWeaponData.prefab, this.transform);
        obj.transform.localPosition = this.transform.localPosition + weaponPosition;
        Weapon_Item weapon = obj.AddComponent<Weapon_Item>();
        weapon.data = GameManager.Inst.WeaponData[(WeaponType)id];
        GameManager.Inst.MainPlayer.StatusUpdate(weapon);
    }

    public void UnEquipWeapon()
    {
        GameObject currentWeapon = transform.GetChild(0).gameObject;
        Destroy(currentWeapon);
    }

    int weaponOrder = 0;
    public uint currentWeapon(int input)
    {
        
        if (input == 1)
        {
            weaponOrder++;
            weaponOrder %= 3;
        }
        if(input == -1)
        {
            if (weaponOrder != 0)
            {
                weaponOrder--;
                weaponOrder &= 3;
            }
            else
            {
                weaponOrder = 2;
            }
        }
        return (uint)weaponOrder;
    }

}

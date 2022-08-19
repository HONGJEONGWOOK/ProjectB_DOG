using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using UnityEngine.SceneManagement;

// 무기 종합

public class Weapons : MonoBehaviour
{
    WCActions weaponChange; // 무기변경 인풋 가져오기
    Animator anim;         // 애니메이터 가져오기

    // WeaponInput 변수
    int weaponOrder = 0;
    int rotate = 0;     // 무기칸 회전 값

    


    //public GameObject weaponSlots;
    //public GameObject equipIcon;
    //bool equip;   // 장비 완전 해제 가능하면 필요. 현재로썬 필요없음

    WeaponData playerWeaponData;

    public WeaponData PlayerWeaponData          // 플레이어 무기데이터를 받아와서 설정
    {
        get => playerWeaponData;
        private set
        {
            if(playerWeaponData != value)
            {
                playerWeaponData = value;
            }
        }
    }


    Player_Hero player;


    // 무기별 스킬 개수는 3개



    private void Awake()
    {
        weaponChange = new();   // 무기전환용 인풋
        anim = GetComponent<Animator>();

        
    }

    private void Start()
    {
        //equipIcon = GameObject.Find("EquipIcon");
        //equipIcon.SetActive(false);
    }



    private void OnEnable()
    {
        weaponChange.Weapons.Enable();
        weaponChange.Weapons.RotateDirection.performed += OnWeaponInput;
    }
    private void OnDisable()
    {
        weaponChange.Weapons.RotateDirection.performed -= OnWeaponInput;
        weaponChange.Weapons.Disable();
    }


    public void openWindow(bool equipWindow = false )
    {
        if(equipWindow==true)       // 장비창 오픈했을 때(평상시에 장비창 입력 추가)
        {
            //WeaponSlots.weaponEquipIcon();
        }
    }


    private void OnWeaponInput(InputAction.CallbackContext context)
    {
        rotate = (int)context.ReadValue<float>();  
        if(rotate==1)   // 시계 방향 회전
        {
            weaponOrder++;
            weaponOrder %= 3;

            anim.SetTrigger("Clockwise");
            //Debug.Log($"{weaponType}");
            //Debug.Log("clockroate");

        }

        if(rotate==-1)  // 반시계 방향 회전
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

            anim.SetTrigger("CounterClockwise");
            //Debug.Log($"{weaponType}");
            //Debug.Log("counterclockroate");
        }


        // WeaponSlots에서 SlotExtansion 찾아와 실행하기.
        //weaponSlots.GetComponent<WeaponSlots>().SlotExtansion(weaponType);    소지 무기 전부 펼치기. 현재 구현 중지

        //ActiveWeapon(weaponType);
        //Debug.Log($"{weaponOrder}");
        IntToWeaponData((uint)weaponOrder);

    }
    public void AssignWeapon(WeaponData weaponData)
    {
        PlayerWeaponData = weaponData;
    }


    public void IntToWeaponData(uint weaponOrder)        // 변경되어 인풋에서 나온 무기순서 (WeaponType클래스)
    {
        WeaponType type = (WeaponType)weaponOrder;
            //    GameObject weapon =transform.GetChild(1).gameObject;
    //    GameObject obj = Instantiate(weapons.PlayerWeaponData.prefab, weapon.transform);
        //GetComponent<Sword>



        GameObject obj = new();
        Weapon_Item weapon = obj.AddComponent<Weapon_Item>();
        weapon.data = GameManager.Inst.WeaponData[(WeaponType)type];
        AssignWeapon(weapon.data);



        GameObject weaponObject = GameObject.Find("Player"). transform.GetChild(1).gameObject;
        Instantiate(PlayerWeaponData.prefab, weaponObject.transform);


    }

}




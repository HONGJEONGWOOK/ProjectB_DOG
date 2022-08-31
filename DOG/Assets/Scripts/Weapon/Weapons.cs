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

    Transform weaponParent;
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
    // 무기별 스킬 개수는 3개

    private void Awake()
    {
        weaponChange = new();   // 무기전환용 인풋
        anim = GetComponent<Animator>();

    }

    private void Start()
    {
        weaponParent = GameManager.Inst.MainPlayer.transform.GetChild(1);
        EquipWeapon(0);
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


    public void openWindow(bool equipWindow = false)
    {
        if (equipWindow == true)       // 장비창 오픈했을 때(평상시에 장비창 입력 추가)
        {
            //WeaponSlots.weaponEquipIcon();
        }
    }


    private void OnWeaponInput(InputAction.CallbackContext context)
    {
        rotate = (int)context.ReadValue<float>();
        if (rotate == 1)   // 시계 방향 회전
        {
            weaponOrder++;
            weaponOrder %= 3;

            anim.SetTrigger("Clockwise");
            //Debug.Log($"{weaponType}");
            //Debug.Log("clockroate");
        }

        if (rotate == -1)  // 반시계 방향 회전
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

        // 해제
        GameObject currentWeapon = weaponParent.GetChild(0).gameObject;         
        Destroy(currentWeapon);                                            


        // 장착
        EquipWeapon((uint)weaponOrder);
    }

    public void AssignWeapon(WeaponData weaponData)
    {
        PlayerWeaponData = weaponData;
    }



    // 다른 스크립트로 옮겨야함
    Vector3 weaponPosition = new Vector3(-0.5f,-0.6f,0f);   // 무기의 위치
    public void EquipWeapon(uint id)        // Weapon 오브젝트에 이미 무기가 들어있을 경우, 무기가 두 개씩 표시되는 문제가 있음
    {
        
        AssignWeapon(GameManager.Inst.WeaponData[id]);
        GameObject obj = Instantiate(PlayerWeaponData.prefab, weaponParent );
        obj.transform.localPosition = weaponParent.transform.localPosition + weaponPosition;
        // Sword 프리팹의 위치가 이상해서 리셋함


        Weapon_Item weapon = obj.AddComponent<Weapon_Item>();
        weapon.data = GameManager.Inst.WeaponData[(WeaponType)id];
        GameManager.Inst.MainPlayer.StatusUpdate(weapon);

    }


}




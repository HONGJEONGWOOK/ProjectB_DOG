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

    WeaponType weaponType = 0;  // 초기 무기는 검

    int rotate = 0;     // 무기칸 회전 값

    public GameObject weaponSlots;

    bool equip;

    // 무기별 스킬 개수는 3개



    private void Awake()
    {
        weaponChange = new();   // 무기전환용 인풋
        anim = GetComponent<Animator>();
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
            if((int)weaponType<=1)  
            {
                weaponType++;       // enum 증가
            }
            else
            {
                weaponType = WeaponType.Sword;  // bow이후엔 sword로
            }
            anim.SetTrigger("Clockwise");
            Debug.Log($"{weaponType}");
            Debug.Log("clockroate");
        }

        if(rotate==-1)  // 반시계 방향 회전
        {
            if ((int)weaponType >=1)
            {
                weaponType--;       // enum 감소
            }
            else
            {
                weaponType = WeaponType.Bow;    // sword이후엔 bow로
            }
            anim.SetTrigger("CounterClockwise");
            Debug.Log($"{weaponType}");
            Debug.Log("counterclockroate");
        }
        
        // WeaponSlots에서 SlotExtansion 찾아와 실행하기. 고칠 방법 찾는중
        weaponSlots.GetComponent<WeaponSlots>().SlotExtansion(weaponType);    


    }

    public void EquipWeapon()
    {
        if(equip==false)
        {

        }
    }

    

    
}

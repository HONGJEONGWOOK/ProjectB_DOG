using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 무기 UI

public class WeaponUI : MonoBehaviour
{
    Animator anim;         // 애니메이터 가져오기
    private void Awake()
    {
        anim = GetComponent<Animator>();

    }

    // 무기 칸 회전
    public void RotateWeaponUI(int input)
    {
        if (input == 1)   // 시계 방향 회전
        {
            anim.SetTrigger("Clockwise");
        }

        if (input == -1)  // 반시계 방향 회전
        {
            anim.SetTrigger("CounterClockwise");
        }
    }
}




using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using UnityEditor;

// 무기 5개 전개창

public class WeaponSlots : MonoBehaviour
{
    Animator anim;
    WeaponType weaponType = 0;

    float delay = 0.25f;

    public GameObject[] swords;
    public GameObject[] daggers;
    public GameObject[] bows;

    //SpriteRenderer sr;



    private void Awake()
    {
        anim = GetComponent<Animator>();

        
        //sr = GetComponent<SpriteRenderer>();
    }

    public void SlotExtansion()
    {
        if ((int)weaponType <= 1)
        {
            weaponType++;       // enum 증가
        }
        else
        {
            weaponType = WeaponType.Sword;  // bow이후엔 sword로
        }
        if ((int)weaponType >= 1)
        {
            weaponType--;       // enum 감소
        }
        else
        {
            weaponType = WeaponType.Bow;    // sword이후엔 bow로
        }
        StartCoroutine(SlotOpenDelay());
    }

    IEnumerator SlotOpenDelay()     // 장비 슬롯이 약간 늦게 펴지도록
    {
       // sr.color = Color.clear;
        //transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(delay);
        transform.GetChild(0).gameObject.SetActive(true);
        anim.SetTrigger("WeaponSellected");
        //anim.Play("Sellected");

        switch (weaponType)         // 무기 종류에 맞춰서 표시
        {
            case WeaponType.Sword:
                for(int i=0;i<=4;i++)
                {
                    swords[i].SetActive(true);
                    daggers[i].SetActive(false);
                    bows[i].SetActive(false);
                }
                break;
            case WeaponType.Dagger:
                for (int i = 0; i <= 4; i++)
                {
                    swords[i].SetActive(false);
                    daggers[i].SetActive(true);
                    bows[i].SetActive(false);
                }
                break;
            case WeaponType.Bow:
                for (int i = 0; i <= 4; i++)
                {
                    swords[i].SetActive(false);
                    daggers[i].SetActive(false);
                    bows[i].SetActive(true);
                }
                break;
            default:
                break;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 무기 창관리(구현 중지)

public class WeaponSlots : MonoBehaviour
{
    //Animator anim;

    //float delay = 0.25f;
    //public GameObject[] swords;
    //public GameObject[] daggers;
    //public GameObject[] bows;



    //private void Awake()
    //{
    //    anim = GetComponent<Animator>();
    //}

    //public void SlotExtansion(WeaponType weaponType)
    //{
    //    StartCoroutine(SlotOpenDelay(weaponType));
    //}

    //IEnumerator SlotOpenDelay(WeaponType weaponType)     // 장비 슬롯이 약간 늦게 펴지도록
    //{
    //    //transform.GetChild(0).gameObject.SetActive(false);
    //    yield return new WaitForSeconds(delay);
    //    transform.GetChild(0).gameObject.SetActive(true);
    //    anim.SetTrigger("WeaponSellected");
    //    switch (weaponType)         // 무기 종류에 맞춰서 표시
    //    {
    //        case WeaponType.Sword:
    //            for (int i = 0; i <= 4; i++)
    //            {
    //                swords[i].SetActive(true);
    //                daggers[i].SetActive(false);
    //                bows[i].SetActive(false);
    //            }
    //            break;
    //        case WeaponType.Dagger:
    //            for (int i = 0; i <= 4; i++)
    //            {
    //                swords[i].SetActive(false);
    //                daggers[i].SetActive(true);
    //                bows[i].SetActive(false);
    //            }
    //            break;
    //        case WeaponType.Bow:
    //            for (int i = 0; i <= 4; i++)
    //            {
    //                swords[i].SetActive(false);
    //                daggers[i].SetActive(false);
    //                bows[i].SetActive(true);
    //            }
    //            break;
    //        default:
    //            break;
    //    }
    //}

}

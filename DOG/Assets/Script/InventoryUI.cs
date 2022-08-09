using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


// 인벤토리 입력 처리

public class InventoryUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    uint id;
    public uint ID { get => id; }

    Button[] slotButton;

    WeaponInfoWindow info;

    Weapons weapons;
   
    //public WeaponInfoWindow WeaponInfoWindow { get =>}

    private void Awake()
    {
        slotButton= GetComponentsInChildren<Button>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 클릭시 장착

        weapons.EquipWeapon();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 마우스 올릴시 세부정보
        //WeaponData data;
        //info.WInfoOpen();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        info.WInfoClose();
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


// �κ��丮 �Է� ó��

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
        // Ŭ���� ����

        weapons.EquipWeapon();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // ���콺 �ø��� ��������
        //WeaponData data;
        //info.WInfoOpen();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        info.WInfoClose();
    }


}

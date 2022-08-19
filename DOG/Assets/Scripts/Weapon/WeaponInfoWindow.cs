using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 무기 정보창
public class WeaponInfoWindow : MonoBehaviour
{
    TextMeshProUGUI weaponName;
    //TextMeshProUGUI weaponType;
    TextMeshProUGUI strength;
    TextMeshProUGUI dexterity;
    TextMeshProUGUI accuracy;

    WeaponData weaponData;

    CanvasGroup canvasGroup;

    private void Awake()
    {
        weaponName = transform.Find("Name").GetComponent<TextMeshProUGUI>();
        //weaponType = transform.Find("Name").GetComponent<TextMeshProUGUI>();
        //strength = transform.Find("Strength").GetComponent<TextMeshProUGUI>();
        //dexterity = transform.Find("Dexterity").GetComponent<TextMeshProUGUI>();
        //accuracy = transform.Find("Accuracy").GetComponent<TextMeshProUGUI>();
    }

    public void WInfoOpen(WeaponData data)
    {
        canvasGroup.alpha = 1;
        weaponData = data;
        Apply();
    }

    public void WInfoClose()
    {
        canvasGroup.alpha = 0;
    }



    public void Apply()
    {
        if(weaponData != null)
        {
            //weaponName.text = weaponData.name;
            //strength.text = weaponData.strength.ToString();
            //dexterity.text = weaponData.dexterity.ToString();
            //accuracy.text = weaponData.accuracy.ToString();
        }
    }

}

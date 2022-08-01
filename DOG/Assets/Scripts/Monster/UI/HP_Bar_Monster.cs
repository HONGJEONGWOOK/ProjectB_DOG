using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HP_Bar_Monster : MonoBehaviour
{
    Slider slider;
    IHealth target;
    GameObject monster;
    TextMeshProUGUI HPText;

    float ratio = 1f;
    float lerpRate = 2.0f;


    //float Timer = 0.0f;
    //float HPShowTime = 3.0f;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        HPText = GetComponentInChildren<TextMeshProUGUI>();

        monster = transform.parent.parent.gameObject;
        target = monster.GetComponent<IHealth>();

        target.onHealthChange += ShowHP;
    }

    private void LateUpdate()
    {
        if (gameObject.activeSelf)
        {
            LerpHP();
            HPText.text = $"{target.HP} / {target.MaxHP}";
        }
    }

    void LerpHP()
    {
        ratio = target.HP / target.MaxHP;

        if (slider.value > 0.3f)
        {
            lerpRate = 0.1f;
        }
        else
        {
            lerpRate = 0.3f;
        }
        slider.value = Mathf.Lerp(slider.value, ratio, lerpRate);
    }

    private void ShowHP()
    {
        this.gameObject.SetActive(true);
    }
    private void HideHP()
    {
        this.gameObject.SetActive(false);
    }

    void CheckStatus()
    {

    }
}
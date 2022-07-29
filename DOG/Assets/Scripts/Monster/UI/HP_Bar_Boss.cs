using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HP_Bar_Boss : MonoBehaviour
{
    Slider slider;
    Image fillImg;
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
        fillImg = transform.Find("Fill Area").GetChild(0).GetComponent<Image>();
        HPText = GetComponentInChildren<TextMeshProUGUI>();

        monster = transform.parent.parent.gameObject;
        target = monster.GetComponent<IHealth>();

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

        if (slider.value > 0.7f)
        {
            lerpRate = 0.1f;
            fillImg.color = Color.green;
        }
        else if( slider.value > 0.5f)
        {
            lerpRate = 0.1f;
            fillImg.color = Color.yellow;
        }
        else if (slider.value > 0.3f)
        {
            lerpRate = 0.05f;
            fillImg.color = new Color(1f, 0.7f, 1f);
        }
        else
        {
            lerpRate = 0.03f;
            fillImg.color = Color.red;
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
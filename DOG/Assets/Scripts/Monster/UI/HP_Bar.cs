using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP_Bar : MonoBehaviour
{
    Transform pivot = null;
    IHealth target = null;
    GameObject monster = null;

    float ratio = 0.0f;
    float lerpRate = 2.0f;
    float Timer = 0.0f;
    float HPShowTime = 3.0f;

    private void Awake()
    {
        pivot = transform.Find("Pivot");
        target = transform.parent.GetComponentInChildren<IHealth>();
        monster = transform.parent.GetChild(0).gameObject;

        target.onHealthChange += ShowHP;
    }

    private void LateUpdate()
    {
        this.transform.position = monster.transform.position;
        if (target != null)
        {
            LerpHP();
        }
    }

    void LerpHP()
    {
        ratio = target.HP / target.MaxHP;

        if (pivot.localScale.x > 0.3f)
        {
            lerpRate = 2.0f;
        }
        else
        {
            lerpRate = 5.0f;
        }
        pivot.localScale = new(Mathf.Lerp(pivot.localScale.x, ratio, lerpRate * Time.deltaTime), 1, 1);
    }

    public void ShowHP()
    {
        this.gameObject.SetActive(true);
    }
    public void HideHP()
    {
        this.gameObject.SetActive(false);
    }
}
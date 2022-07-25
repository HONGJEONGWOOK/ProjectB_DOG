using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP_Bar : MonoBehaviour
{
    Transform pivot = null;
    IHealth target = null;

    GameObject monster = null;

    private void Awake()
    {
        pivot = transform.Find("Pivot");
        target = transform.parent.GetComponentInChildren<IHealth>();
        monster = transform.parent.GetChild(0).gameObject;

        target.onHealthChange += HPChange;
    }

    private void LateUpdate()
    {
        this.transform.position = monster.transform.position;
    }

    void HPChange()
    {
        if (target != null)
        {
            float ratio = target.HP / target.MaxHP;
            pivot.localScale = new(ratio, 1, 1);
        }
    }
}

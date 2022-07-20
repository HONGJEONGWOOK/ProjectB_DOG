using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP_Bar : MonoBehaviour
{
    Transform pivot = null;
    IHealth target = null;

    private void Awake()
    {
        pivot = transform.Find("Pivot");
        target = GetComponentInParent<IHealth>();

        target.onHealthChange += HPChange;

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

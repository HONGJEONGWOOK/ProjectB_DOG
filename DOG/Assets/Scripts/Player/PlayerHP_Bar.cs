using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP_Bar : MonoBehaviour
{
    IHealth target;
    Slider slider;


    private void Start()
    {
        slider = GetComponent<Slider>();
        target = PlayerManager.Inst.MainPlayer.GetComponent<IHealth>();
        target.onHealthChange += SetHP_Value;


    }

    private void SetHP_Value()
    {
        if (target != null)
        {
            float ratio = target.HP / target.MaxHP;
            slider.value = ratio;
        }
    }
}

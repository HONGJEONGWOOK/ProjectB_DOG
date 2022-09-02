using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldToVillage : MonoBehaviour
{
    public System.Action OnVillageEnter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnVillageEnter?.Invoke();   // 트리거 활성화
        }
    }
}

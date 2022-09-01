using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageToField : MonoBehaviour
{
    public System.Action OnFieldEnter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnFieldEnter?.Invoke(); // 트리거 활성화
        }
    }
}

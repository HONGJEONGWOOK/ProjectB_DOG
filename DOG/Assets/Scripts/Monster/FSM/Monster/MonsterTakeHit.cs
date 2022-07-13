using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster.Enums;

public class MonsterTakeHit : MonoBehaviour
{
    Monsters goblin = null;

    private void Awake()
    {
        goblin = gameObject.transform.Find("Goblin").GetComponent<Monsters>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            goblin.status = MonsterCurrentState.HIT;
        }
    }
}

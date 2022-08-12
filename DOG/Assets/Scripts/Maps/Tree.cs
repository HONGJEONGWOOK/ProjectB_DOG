using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    SpriteRenderer tree;


    private void Awake()
    {
        tree = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        tree.color = new Color(1, 1, 1, 0.5f);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        tree.color = new Color(1, 1, 1, 1f);
    }
}

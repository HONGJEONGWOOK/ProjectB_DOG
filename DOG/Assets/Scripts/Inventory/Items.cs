using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    [HideInInspector] public ItemData data;

    private float moveDistance = 0.005f;
    private float moveSpeed = 2.0f;

    float timer = 0;

    private void Update()
    {
        timer += Time.deltaTime;
        transform.localPosition = new Vector3(transform.position.x, transform.position.y + moveDistance * Mathf.Sin(moveSpeed * timer), 0) ;
    }

}

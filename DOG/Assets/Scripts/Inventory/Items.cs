using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{

    public ItemData data;
    private float moveDistance = 1.0f;
    

    private float moveDistance = 0.2f;
    private float moveSpeed = 2.0f;


    float timer = 0;
    
    private void Update()
    {
        timer += Time.deltaTime;
        transform.localPosition = new Vector3(transform.position.x, moveDistance * Mathf.Abs(Mathf.Sin(moveSpeed * timer)), 0) ;
    }

}

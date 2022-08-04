using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalKey : MonoBehaviour
{
    float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        transform.position = new Vector2(transform.position.x, Mathf.Abs(Mathf.Sin(timer)));
    }
}

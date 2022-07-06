using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    private static MonsterManager instance = null;
    public static MonsterManager Inst { get => instance; }

    private CircleCollider2D detectCollider = null;
    private Transform target = null;

    public CircleCollider2D DetectCollider { get => detectCollider; }
    public Transform Target { get => target; }
    private void Awake()
    {
        if (instance = null)
        {
            instance = this;
            instance.Initialize();
        }
        else
        {
            if (instance != null)
            {
                Destroy(this.gameObject);
            }
        }

        detectCollider = FindObjectOfType<MonsterManager>().GetComponent<CircleCollider2D>();
        //target = FindObjectOfType<Player_Move>().GetComponent<Transform>();

    }

    private void Initialize()
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Detected");
    }
}

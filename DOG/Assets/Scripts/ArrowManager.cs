using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    private static ArrowManager arrow_Instance;
    public Queue<GameObject> arrows;
    public GameObject arrow;
    private int ArrowNum = 30;
    private int arrowDirecction = 1;

    public static ArrowManager Arrow_Instance { get => arrow_Instance; }

    public int ArrowDirection { get => arrowDirecction; set { arrowDirecction = value; } }
    void Awake()
    {
        if (arrow_Instance == null)
        {
            arrow_Instance = this;
            arrow_Instance.Initialize();
        }
        else
        {
            if (arrow_Instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void Start()
    {
        arrows = new Queue<GameObject>();
        GameObject tmp;
        for (int i = 0; i < ArrowNum; i++)
        {
            tmp = Instantiate(arrow, this.gameObject.transform);
            tmp.SetActive(false);
            arrows.Enqueue(tmp);
        }
    }

    void Initialize()
    {

    }



    public GameObject GetPooledArrow()
    {
        if (arrows.Count > 0)
        {
            GameObject arr = arrows.Dequeue();
            arr.SetActive(true);
            //arrow onEnable 작동 -> 방향 결정
            return arr;
        }
        return null;
    }

    public void ReturnPooledArrow(GameObject uselessArrow)
    {
        uselessArrow.SetActive(false);
        arrows.Enqueue(uselessArrow);
    }
}

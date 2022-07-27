using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletManager : MonoBehaviour
{
    private static EnemyBulletManager instance;
    private Queue<GameObject> arrows;
    private Queue<GameObject> fireBalls;
    public GameObject arrow;
    public GameObject fireBall;
    private int ArrowNum = 30;
    private int fireBallNum = 15;

    public static EnemyBulletManager Inst { get => instance; }

    private int arrowDirection = 1;
    public int ArrowDirection { get => arrowDirection; set { arrowDirection = value; } }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            instance.Initialize();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void Initialize()
    {
        
    }

    private void Start()
    {
        arrows = new Queue<GameObject>();
        GameObject tmp;
        for (int i = 0; i < ArrowNum; i++)
        {
            tmp = Instantiate(arrow, this.gameObject.transform);
            tmp.SetActive(false);
            arrows.Enqueue(tmp);
        }

        fireBalls = new Queue<GameObject>();
        GameObject tmp_Fire;
        for (int i = 0; i < fireBallNum; i++)
        {
            tmp_Fire = Instantiate(fireBall, this.gameObject.transform);
            tmp_Fire.SetActive(false);
            fireBalls.Enqueue(tmp_Fire);
        }
    }


    // ##################### Bow Monster #######################
    public GameObject GetPooledArrow()
    {
        if (arrows.Count > 0)
        {
            GameObject arr = arrows.Dequeue();
            arr.SetActive(true);
            return arr;
        }
        return null;
    }

    public void ReturnPooledArrow(GameObject uselessArrow)
    {
        uselessArrow.SetActive(false);
        arrows.Enqueue(uselessArrow);
    }

    // ##################### Boss Monster ##################################
    public GameObject GetPooledFireBall()
    {
        if (fireBalls.Count > 0)
        {
            GameObject arr = fireBalls.Dequeue();
            arr.SetActive(true);
            return arr;
        }
        return null;
    }

    public void ReturnPooledFireBall(GameObject uselessFireBall)
    {
        uselessFireBall.SetActive(false);
        arrows.Enqueue(uselessFireBall);
    }
}

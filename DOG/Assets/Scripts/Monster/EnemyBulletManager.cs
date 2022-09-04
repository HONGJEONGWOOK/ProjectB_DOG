using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletManager : MonoBehaviour
{
    private static EnemyBulletManager instance;
    public static EnemyBulletManager Inst { get => instance; }

    [SerializeField] private ObjectPoolingData[] poolingObjects;
        // [0] : Arrows
        // [1] : Meteors

    private static Dictionary<int, Queue<GameObject>> pooledObjects = new();
    public static Dictionary<int, Queue<GameObject>> PooledObjects => pooledObjects;

    // ---------------- Arrows -----------------
    private Queue<GameObject> arrows;
    Transform arrowParent;

    // ---------------- Meteor -----------------
    Transform meteorParent;
    private Queue<GameObject> meteors;

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
        arrowParent = transform.GetChild(0);
        meteorParent = transform.GetChild(1);


        // ------------------- Arrow -----------------------
        arrows = new Queue<GameObject>();
        pooledObjects.Add((int)ProjectileID.Arrows , arrows);

        GameObject tmp;
        for (int i = 0; i < poolingObjects[0].poolSize; i++)
        {
            tmp = Instantiate(poolingObjects[0].prefab, arrowParent);
            pooledObjects[(int)ProjectileID.Arrows].Enqueue(tmp);
            tmp.SetActive(false);
        }

        // ------------------- Meteor -----------------------
        meteors = new Queue<GameObject>();
        pooledObjects.Add((int)ProjectileID.Meteor, meteors);
        GameObject tmp_Fire;
        for (int i = 0; i < poolingObjects[1].poolSize; i++)
        {
            tmp_Fire = Instantiate(poolingObjects[1].prefab, meteorParent);
            pooledObjects[(int)ProjectileID.Meteor].Enqueue(tmp_Fire);
            tmp_Fire.SetActive(false);
        }
    }


    // ##################### Method #######################
    public GameObject GetPooledObject(Queue<GameObject> poolingObject)
    {
        if (poolingObject.Count > 0)
        {
            GameObject obj = poolingObject.Dequeue();
            //obj.SetActive(true);
            return obj;
        }
        else
        {
            // 없을 경우 하나 더 생성
        }
        return null;
    }

    public void ReturnPooledEnemy(Queue<GameObject> returnQueue, GameObject uselessObject)
    {
        returnQueue.Enqueue(uselessObject);
        uselessObject.SetActive(false);
        uselessObject.transform.position = Vector2.zero;
    }
}

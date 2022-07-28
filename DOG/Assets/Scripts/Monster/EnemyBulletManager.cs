using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletManager : MonoBehaviour
{
    private static EnemyBulletManager instance;

    private Dictionary<string, Queue<GameObject>> pooledObjects = new();
    private Queue<GameObject> arrows;
    private Queue<GameObject> meteors;

    public GameObject[] poolingPrefabs;
    [SerializeField] private int ArrowNum = 30;
    [SerializeField] private int meteorNum = 15;

    Transform ArrowParent;
    Transform MeteorParent;

    public static EnemyBulletManager Inst { get => instance; }

    public Dictionary<string, Queue<GameObject>> PooledObjects => pooledObjects;

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
        ArrowParent = transform.GetChild(0);
        MeteorParent = transform.GetChild(1);

        // ------------------- Arrow -----------------------

        arrows = new Queue<GameObject>();
        pooledObjects.Add(poolingPrefabs[0].name, arrows);

        GameObject tmp;
        for (int i = 0; i < ArrowNum; i++)
        {
            tmp = Instantiate(poolingPrefabs[0], ArrowParent);
            pooledObjects[poolingPrefabs[0].name].Enqueue(tmp);
            tmp.SetActive(false);
        }

        // ------------------- Meteor -----------------------
        meteors = new Queue<GameObject>();
        pooledObjects.Add(poolingPrefabs[1].name, meteors);
        GameObject tmp_Fire;
        for (int i = 0; i < meteorNum; i++)
        {
            tmp_Fire = Instantiate(poolingPrefabs[1], MeteorParent);
            pooledObjects[poolingPrefabs[1].name].Enqueue(tmp_Fire);
            tmp_Fire.SetActive(false);
        }
    }

    // ##################### Bow Monster #######################
    public GameObject GetPooledObject(Queue<GameObject> poolingObject)
    {
        if (poolingObject.Count > 0)
        {
            GameObject obj = poolingObject.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            // 없을 경우 하나 더 생성
        }
        return null;
    }

    public void ReturnPooledObject(Queue<GameObject> returnQueue, GameObject uselessObject)
    {
        returnQueue.Enqueue(uselessObject);
        uselessObject.SetActive(false);
        uselessObject.transform.position = Vector2.zero;
    }
}

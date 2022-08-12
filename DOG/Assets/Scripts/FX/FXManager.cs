using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    private static FXManager instance;
    public static FXManager Inst => instance;

    // Dictionary
    private static Dictionary<int, Queue<GameObject>> pooledFX;
    public static Dictionary<int, Queue<GameObject>> PooledFX => pooledFX;

    // 각 오브젝트
    public ObjectPoolingData[] fxData;
        // [0] : Big Explosion

    private Animator[] anims;

    // --------------------- Big Explosion -------------------------
    Queue<GameObject> bigExplosion;
    private Transform bigExplosionParent;
    private int explosionID;
    public int ExplosionID => explosionID;

    private void Awake()
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
        pooledFX = new();
        bigExplosion = new();

        pooledFX.Add(fxData[0].objectID, bigExplosion);

        bigExplosionParent = transform.GetChild(0);
        explosionID = fxData[0].objectID;

        // ---------------- Big Explosion --------------------------------
        for (int i = 0; i < fxData[0].poolSize; i++)
        {
            GameObject obj = Instantiate(fxData[0].prefab, bigExplosionParent);
            pooledFX[explosionID].Enqueue(obj);
            obj.SetActive(false);
        }
    }

    // ######################### Methods ##########################################
    public GameObject GetPooledFX(Queue<GameObject> pooledQueue)
    {
        if (pooledQueue.Count > 0)
        {
            GameObject fx = pooledQueue.Dequeue();
            //fx.SetActive(true);
            return fx;
        }
        else
        {
            // 생성
        }
        return null;
    }

    public void ReturnFX(Queue<GameObject> returningQueue, GameObject uselessFX)
    {
        returningQueue.Enqueue(uselessFX);
        uselessFX.SetActive(false);
        uselessFX.transform.position = Vector2.zero;
    }
}

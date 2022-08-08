using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    private static MonsterManager instance = null;
    public static MonsterManager Inst { get => instance; }

    [SerializeField] private ObjectPoolingData[] poolingMonsters;
        // [0] : Goblin
        // [1] : Treant
        // [2] : Boss

    private static Dictionary<int, Queue<GameObject>> pooledMonster = new();
    public static Dictionary<int, Queue<GameObject>> PooledMonster => pooledMonster;

    public ObjectPoolingData[] PoolingMonsters => poolingMonsters;
    // -------- Goblin Data ----------------
    private Queue<GameObject> goblins;
    Transform goblinParent;
    private int goblinID;
    public int GoblinID => goblinID;

    // -------- Treant Data ----------------
    private Queue<GameObject> treants;
    Transform treantParent;
    private int treantID;
    public int TreantID => treantID;

    // -------- Boss Data ----------------
    private Queue<GameObject> bosses;
    Transform bossParent;
    private int bossID;
    public int BossID => bossID;

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
            if (instance != null)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void Initialize()
    {
        goblinParent = transform.GetChild(0);
        goblinID = poolingMonsters[(int)MonsterID.GOBLIN].objectID;

        treantParent = transform.GetChild(1);
        treantID = poolingMonsters[(int)MonsterID.TREANT].objectID;

        bossParent = transform.GetChild(2);
        bossID = poolingMonsters[(int)MonsterID.BOSS].objectID;

        // --------------------- Goblin --------------------------------
        goblins = new();
        pooledMonster.Add(goblinID, goblins);

        for (int i = 0; i < poolingMonsters[goblinID].poolSize; i++)
        {
            GameObject gob = Instantiate(poolingMonsters[goblinID].prefab, goblinParent);
            pooledMonster[goblinID].Enqueue(gob);
            gob.SetActive(false);
        }


        // --------------------- Treant --------------------------------
        treants = new();
        pooledMonster.Add(treantID, treants);

        for (int i = 0; i < poolingMonsters[TreantID].poolSize; i++)
        {
            GameObject trea = Instantiate(poolingMonsters[TreantID].prefab, treantParent);
            pooledMonster[treantID].Enqueue(trea);
            trea.SetActive(false);
        }

        // --------------------- Boss -----------------------------------
        bosses = new();
        pooledMonster.Add(bossID, bosses);

        for (int i = 0; i < poolingMonsters[bossID].poolSize; i++)
        {
            GameObject boss = Instantiate(poolingMonsters[bossID].prefab, bossParent);
            pooledMonster[bossID].Enqueue(boss);
            boss.SetActive(false);
        }
    }

    // ######################### Methods ##########################################
    public GameObject GetPooledMonster(Queue<GameObject> poolingQueue)
    {
        if (poolingQueue.Count > 0)
        {
            GameObject monster = poolingQueue.Dequeue();
            //monster.SetActive(true);
            return monster;
        }
        else
        {

        }
        return null;
    }

    public void ReturnPooledMonster(Queue<GameObject> returningQueue, GameObject uselessMonster)
    {
        returningQueue.Enqueue(uselessMonster);
        uselessMonster.SetActive(false);
        uselessMonster.transform.position = Vector2.zero;
        uselessMonster.transform.localScale = new Vector2(1f, 1f);
    }
}

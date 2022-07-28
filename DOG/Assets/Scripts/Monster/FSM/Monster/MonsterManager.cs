using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    private static MonsterManager instance = null;
    public static MonsterManager Inst { get => instance; }

    public GameObject[] poolingMonsters;
    [SerializeField] private int goblinNum = 15;
    [SerializeField] private int treantNum = 15;
    [SerializeField] private int bossNum = 2;
    Transform goblinParent;
    Transform treantParent;
    Transform bossParent;

    private Dictionary<string, Queue<GameObject>> pooledMonster = new();

    private Queue<GameObject> goblins;
    private Queue<GameObject> treants;
    private Queue<GameObject> bosses;

    public Dictionary<string, Queue<GameObject>> PooledMonster => pooledMonster;

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
        treantParent = transform.GetChild(1);
        bossParent = transform.GetChild(2);

        // --------------------- Goblin --------------------------------
        goblins = new();
        pooledMonster.Add(poolingMonsters[0].name, goblins);

        for (int i = 0; i < goblinNum; i++)
        {
            GameObject gob = Instantiate(poolingMonsters[0], goblinParent);
            pooledMonster[poolingMonsters[0].name].Enqueue(gob);
            gob.SetActive(false);   
        }


        // --------------------- Treant --------------------------------
        treants = new();
        pooledMonster.Add(poolingMonsters[1].name, treants);

        for (int i = 0; i < treantNum; i++)
        {
            GameObject trea = Instantiate(poolingMonsters[1], treantParent);
            pooledMonster[poolingMonsters[1].name].Enqueue(trea);
            trea.SetActive(false);
        }

        // --------------------- Boss -----------------------------------
        bosses = new();
        pooledMonster.Add(poolingMonsters[2].name, bosses);

        for (int i = 0; i < bossNum; i++)
        {
            GameObject boss = Instantiate(poolingMonsters[2], bossParent);
            pooledMonster[poolingMonsters[2].name].Enqueue(boss);
            boss.SetActive(false);
        }
    }

    // ######################### Methods ##########################################
    public GameObject GetPooledMonster(Queue<GameObject> poolingQueue)
    {
        if (poolingQueue.Count > 0)
        {
            GameObject monster = poolingQueue.Dequeue();
            monster.SetActive(true);
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
    }
}

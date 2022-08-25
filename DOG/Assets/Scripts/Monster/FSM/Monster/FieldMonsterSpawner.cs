using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FieldMonsterSpawner : MonoBehaviour
{
    SpawnPoint[] spawnPoint;
    MonsterID type;
    GameObject spawnedMonster;

    private uint monsterPopulation = 0;

    [SerializeField] float spawnCoolTime = 5.0f;
    WaitForSeconds spawnTimer;

    private void Awake()
    {
        //spawnPoint = transform.GetComponentsInChildren<Transform>().Skip(1).ToArray();
        spawnPoint = transform.GetComponentsInChildren<SpawnPoint>();

        spawnTimer = new WaitForSeconds(spawnCoolTime);
    }

    private void Start()
    {
        foreach (SpawnPoint t in spawnPoint)
        {
            SpawnMonster(t);
            monsterPopulation++;
            t.hasMonster = true;
            t.monsterType = type;
            t.monster = spawnedMonster;
        }

        StartCoroutine(Respawner());
    }

    void SpawnMonster(SpawnPoint spawnPoint)
    {
        int randMonster = Random.Range(0, (int)System.Enum.GetValues(typeof(MonsterID)).Cast<MonsterID>().Last());
        switch(randMonster)
        {
            case 0:
                type = MonsterID.GOBLIN;
                break;
            case 1:
                type = MonsterID.TREANT;
                break;
        }
        spawnedMonster = MonsterManager.GetPooledMonster(MonsterManager.PooledMonster[randMonster]);
        spawnedMonster.transform.position = spawnPoint.transform.position;
        spawnPoint.monster = spawnedMonster;

        spawnedMonster.SetActive(true);
    }


    IEnumerator Respawner()
    {
        while (true)
        {
            if (monsterPopulation < spawnPoint.Length + 1)
            {
                foreach(SpawnPoint t in spawnPoint)
                {
                    if (!t.monster.transform.GetChild(0).gameObject.activeSelf)
                    {
                        t.hasMonster = false;
                    }

                    if (t.hasMonster == false)
                    {
                        SpawnMonster(t);
                        t.hasMonster = true;
                    }
                }
            }
            yield return spawnTimer;
        }
    }
}

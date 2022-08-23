using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FieldMonsterSpawner : MonoBehaviour
{
    SpawnPoint[] spawnPoint;

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
            SpawnMonster(t.transform);
            monsterPopulation++;
            t.hasMonster = true;
        }

        StartCoroutine(Respawner());
    }

    void SpawnMonster(Transform spawnPoint)
    {
        int randMonster = Random.Range(0, (int)System.Enum.GetValues(typeof(MonsterID)).Cast<MonsterID>().Last());
        GameObject obj = MonsterManager.GetPooledMonster(MonsterManager.PooledMonster[randMonster]);
        obj.transform.position = spawnPoint.position;
        obj.SetActive(true);
    }

    IEnumerator Respawner()
    {
        while (true)
        {
            if (monsterPopulation < spawnPoint.Length + 1)
            {
                foreach(SpawnPoint t in spawnPoint)
                {
                    if (t.hasMonster == false)
                    {
                        SpawnMonster(t.transform);
                        t.hasMonster = true;
                    }
                }
            }
            yield return spawnTimer;
        }
    }
}

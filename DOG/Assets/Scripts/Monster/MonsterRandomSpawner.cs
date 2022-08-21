using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRandomSpawner : MonoBehaviour
{
    private DivideSpace rooms;

    [SerializeField] private int population = 10;
    [SerializeField] private float size = 0.5f;
    private void Awake()
    {
        rooms = GetComponent<MakeRandomMap>().Divide;
    }

    // ##################################### Method #########################################
    public void SpawnMonster()
    {
        for (int i = 0; i < population; i++)
        {
            // ---- Random Number Generator
            int randMonster = Random.Range(0, MonsterManager.PooledMonster.Count - 1);   // 마지막방은 포탈
            int randRoom = Random.Range(1, rooms.spaceList.Count - 1);                   // 포탈 방 전까지 스폰
            Vector2 randOffset = rooms.MinWidth * 0.5f * 0.2f * Random.insideUnitCircle;

            // ---- Monster Object pool
            GameObject monster = MonsterManager.GetPooledMonster(MonsterManager.PooledMonster[randMonster]);
            monster.transform.position = (Vector2)rooms.spaceList[randRoom].Center() + randOffset;
            monster.transform.localScale = new Vector2(size, size);     // 던전 국한 크기 축소
            monster.SetActive(true);
        }

        //SpawnBoss();
    }

    public void SpawnBoss()
    {
        GameObject boss = MonsterManager.GetPooledMonster(MonsterManager.PooledMonster[(int)MonsterID.BOSS]);
        boss.transform.position = (Vector2)rooms.spaceList[rooms.spaceList.Count - 1].Center();      //마지막 방 전에 보스 배치
        boss.GetComponentInChildren<Canvas>().enabled = true;
    }
}

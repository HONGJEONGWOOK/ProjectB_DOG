using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRandomSpawner : MonoBehaviour
{
    private DivideSpace rooms;

    [SerializeField] private int population = 10;

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
            int randMonster = Random.Range(0, MonsterManager.PooledMonster.Count - 1);   // 마지막은 보스이므로 제외
            int randRoom = Random.Range(1, rooms.spaceList.Count - 2);                   // 보스방 전까지 스폰
            Vector2 randOffset = Random.insideUnitCircle * rooms.MinWidth * 0.5f * 0.4f;

            // ---- Monster Object pool
            GameObject monster = MonsterManager.GetPooledMonster(MonsterManager.PooledMonster[randMonster]);
            monster.transform.position = (Vector2)rooms.spaceList[randRoom].Center() + randOffset;
            monster.transform.localScale = new Vector2(0.5f, 0.5f);     // 던전 국한 크기 축소
            monster.SetActive(true);
        }

        //SpawnBoss();
    }

    public void SpawnBoss()
    {
        GameObject boss = MonsterManager.GetPooledMonster(MonsterManager.PooledMonster[(int)MonsterID.BOSS]);
        boss.transform.position = (Vector2)rooms.spaceList[rooms.spaceList.Count - 2].Center();      //마지막 방 전에 보스 배치
        boss.GetComponentInChildren<Canvas>().enabled = true;
    }
}

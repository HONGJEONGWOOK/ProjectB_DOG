
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeRandomMap : MonoBehaviour
{
    [SerializeField]
    private DivideSpace divideSpace;        // 나누어진 공간들의 리스트인 SpaceList 가져오기 위한 변수
    [SerializeField]
    private SpreadTilemap spreadTilemap;    // 방 복도에 바닥 타일을 깔고 벽에 벽 타일을 깔기위한 변수
    private MonsterRandomSpawner monsterSpawner;


    [SerializeField]
    private int distance;                   
    [SerializeField]
    private int minRoomWidth;
    [SerializeField]
    private int minRoomHeight;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject bossRoomChange;


    private HashSet<Vector2Int> floor;
    private HashSet<Vector2Int> wall;

    public GameObject startPoint;

    // ################################################## Property ############################################
    public DivideSpace Divide => divideSpace;

    private void Awake()
    {
        monsterSpawner = GetComponent<MonsterRandomSpawner>();
    }

    /// <summary>
    ///  시작 시 랜덤맵 생성 시작
    /// </summary>
    private void Start()
    {
        StartRandomMap();
        monsterSpawner.SpawnMonster();
    }


    public void StartRandomMap()
    {
        //spreadTilemap.ClearAllTiles();      // 시작 시 깔려있는 모든 타일 제거
        divideSpace.totalSpace = new RectangleSpace(new Vector2Int(0, 0), divideSpace.totalWidth, divideSpace.totalHeight);
        divideSpace.spaceList = new List<RectangleSpace>();
        floor = new HashSet<Vector2Int>();
        wall = new HashSet<Vector2Int>();
        divideSpace.DivideRoom(divideSpace.totalSpace);     // DivideRoom 함수를 통해 SpaceList 생성
        MakeRandomRooms();  // 방 좌표

        MakeCorridors();    // 복도 좌표

        MakeWall();         // 벽 좌표

        // 위 좌표를 받은 곳에 SpreadFloorTilemap, SpreadWallTilemap를 통해 바닥과 벽 타일을 생성
        spreadTilemap.SpreadFloorTilemap(floor);
        spreadTilemap.SpreadWallTilemap(wall);

        startPoint.transform.position = (Vector2)divideSpace.spaceList[0].Center();
        bossRoomChange.transform.position = (Vector2)divideSpace.spaceList[divideSpace.spaceList.Count - 1].Center();

    }

    /// <summary>
    /// 방을 만드는 함수
    /// </summary>
    private void MakeRandomRooms()
    {
        foreach(var space in divideSpace.spaceList) 
        {
            HashSet<Vector2Int> position = MakeARandomRectangleRoom(space);
            floor.UnionWith(position);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="space"></param>
    /// <returns></returns>
    private HashSet<Vector2Int> MakeARandomRectangleRoom(RectangleSpace space)
    {
        HashSet<Vector2Int> position = new HashSet<Vector2Int>();
        int width = Random.Range(minRoomWidth, space.width + 1 - distance * 2);
        int height = Random.Range(minRoomHeight, space.height + 1 - distance * 2);
        for(int i = space.Center().x - width / 2; i<=space.Center().x + width / 2; i++)
        {
            for(int j = space.Center().y - height / 2; j < space.Center().y + height / 2; j++)
            {
                position.Add(new Vector2Int(i, j));
            }
        }
        return position;
    }

    /// <summary>
    /// 방의 중심이 정해 졌다면 첫번째 방에서 가장 가까운 방의 중심을 연결시키고 처음방은 리스트에서 제거
    /// 이런식으로 마지막 방까지 중심을 이어 통로를 생성
    /// </summary>
    private void MakeCorridors()
    {
        List<Vector2Int> tempCenters = new List<Vector2Int>();
        foreach( var space in divideSpace.spaceList)
        {
            tempCenters.Add(new Vector2Int(space.Center().x, space.Center().y));
        }

        Vector2Int nextCenter;
        Vector2Int currentCenter = tempCenters[0];
        tempCenters.Remove(currentCenter);
        while( tempCenters.Count != 0)
        {
            nextCenter = ChooseShortestNextCorridor(tempCenters, currentCenter);
            MakeOneCorridor(currentCenter, nextCenter);
            currentCenter = nextCenter;
            tempCenters.Remove(currentCenter);
        }
    }

    /// <summary>
    /// 가장 가까운 중심을 찾아주는 함수
    /// </summary>
    /// <param name="tempCenters"></param>
    /// <param name="previousCenter"></param>
    /// <returns></returns>
    private Vector2Int ChooseShortestNextCorridor(List<Vector2Int> tempCenters, Vector2Int previousCenter)
    {
        int n = 0;
        float minLength = float.MaxValue;
        for(int i = 0; i < tempCenters.Count; i++)
        {
            if(Vector2.Distance(previousCenter, tempCenters[i]) < minLength)
            {
                minLength = Vector2.Distance(previousCenter, tempCenters[i]);
                n = i;
            }
        }
        return tempCenters[n];
    }

    /// <summary>
    /// 복도 화된 좌표들을 지정하는 함수
    /// </summary>
    /// <param name="currentCenter"></param>
    /// <param name="nextCenter"></param>
    private void MakeOneCorridor(Vector2Int currentCenter, Vector2Int nextCenter)
    {
        Vector2Int current = new Vector2Int(currentCenter.x, currentCenter.y);
        Vector2Int next = new Vector2Int(nextCenter.x, nextCenter.y);
        floor.Add(current);
        while(current.x != next.x)
        {
            if(current.x < next.x)
            {
                current.x += 1;
                floor.Add(current);
            }
            else
            {
                current.x -= 1;
                floor.Add(current);
            }
        }
        while(current.y != next.y)
        {
            if(current.y < next.y)
            {
                current.y += 1;
                floor.Add(current);
            }
            else
            {
                current.y -= 1;
                floor.Add(current);
            }
        }
    }

    /// <summary>
    /// 벽을 만드는 함수
    /// </summary>
    private void MakeWall()
    {
        foreach( Vector2Int tile in floor)
        {
            HashSet<Vector2Int> boundary = Make3X3Square(tile);
            boundary.ExceptWith(floor); // 벽을 생성하기전 이 부분이 바닥 타일이 아닌지 확인하는 함수
            if(boundary.Count != 0)
            {
                wall.UnionWith(boundary);
            }
        }
    }

    private HashSet<Vector2Int> Make3X3Square(Vector2Int tile)
    {
        HashSet<Vector2Int> boundary = new HashSet<Vector2Int>();
        for(int i = tile.x - 1; i <= tile.x + 1; i++)
        {
            for(int j = tile.y - 1; j<= tile.y + 1; j++)
            {
                boundary.Add(new Vector2Int(i, j));
            }
        }
        return boundary;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps; // 타일맵 변수를 이용하기 위해 추가

public class SpreadTilemap : MonoBehaviour
{
    /// <summary>
    /// 바닥과 벽을 타일맵에 생성하는 역할.
    /// </summary>
    [SerializeField]
    private Tilemap floor;
    [SerializeField]
    private Tilemap wall;

    /// <summary>
    /// 사용할 타일 에셋
    /// </summary>
    [SerializeField]
    private TileBase floorTile;
    [SerializeField]
    private TileBase wallTile;

    
    public void SpreadFloorTilemap(HashSet<Vector2Int> positions)
    {
        SpreadTile(positions, floor, floorTile);
    }
    
    public void SpreadWallTilemap(HashSet<Vector2Int> positions)
    {
        SpreadTile(positions, wall, wallTile);
    }

    /// <summary>
    /// 파라메타로 받은 positions에 있는 좌표들에 타일을 까는 함수
    /// </summary>
    /// <param name="positions"></param>
    /// <param name="tilemap"></param>
    /// <param name="tile"></param>
    private void SpreadTile(HashSet<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach(var position in positions)
        {
            tilemap.SetTile((Vector3Int)position, tile);
        }
    }

    /// <summary>
    /// 타일맵에 깔려있던 타일을 모두 제거
    /// </summary>
    public void ClearAllTiles()
    {
        floor.ClearAllTiles();
        wall.ClearAllTiles();
    }
}

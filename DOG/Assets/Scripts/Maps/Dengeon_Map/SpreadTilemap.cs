using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    private void SpreadTile(HashSet<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach(var position in positions)
        {
            tilemap.SetTile((Vector3Int)position, tile);
        }
    }

    public void ClearAllTiles()
    {
        floor.ClearAllTiles();
        wall.ClearAllTiles();
    }
}

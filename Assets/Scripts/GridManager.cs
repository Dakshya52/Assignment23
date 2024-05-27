using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public int gridSize = 10;
    

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector3 position = new Vector3(x, 0, y);
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
                tile.name = $"Tile_{x}_{y}";
                TileInfo tileInfo = tile.GetComponent<TileInfo>();
                if (tileInfo == null)
                {
                    tileInfo = tile.AddComponent<TileInfo>();
                }
                tileInfo.x = x;
                tileInfo.y = y;

            }
        }
    }
}

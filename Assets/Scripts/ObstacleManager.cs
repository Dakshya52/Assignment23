using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public ObstacleData obstacleData;
    public GameObject obstaclePrefab;

    void Start()
    {
        GenerateObstacles();
    }

    void GenerateObstacles()
    {
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                int index = y * 10 + x;
                if (obstacleData.obstaclePositions[index])
                {
                    Vector3 position = new Vector3(x, 0.5f, y); // Adjust the Y position to place it above the grid
                    Instantiate(obstaclePrefab, position, Quaternion.identity);

                    // Mark the tile as an obstacle
                    RaycastHit hit;
                    if (Physics.Raycast(new Vector3(x, 2, y), Vector3.down, out hit))
                    {
                        TileInfo tileInfo = hit.collider.GetComponent<TileInfo>();
                        if (tileInfo != null)
                        {
                            tileInfo.isObstacle = true;
                        }
                    }
                }
            }
        }
    }
}
using UnityEngine;

public class TileInfo : MonoBehaviour
{
    public int x;
    public int y;
    public bool isObstacle;

    void Start()
    {
        // Check if the tile is tagged as an obstacle
        if (gameObject.CompareTag("Obstacle"))
        {
            isObstacle = true;
        }
    }
}

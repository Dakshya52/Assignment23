using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private static readonly Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int(0, 1),  // up
        new Vector2Int(1, 0),  // right
        new Vector2Int(0, -1), // down
        new Vector2Int(-1, 0)  // left
    };

    public static List<Vector3> FindPath(TileInfo start, TileInfo target)
    {
        if (start == null || target == null)
        {
            return new List<Vector3>();
        }

        Dictionary<TileInfo, TileInfo> cameFrom = new Dictionary<TileInfo, TileInfo>();
        Dictionary<TileInfo, float> costSoFar = new Dictionary<TileInfo, float>();

        List<TileInfo> openList = new List<TileInfo> { start };
        HashSet<TileInfo> closedList = new HashSet<TileInfo>();

        cameFrom[start] = null;
        costSoFar[start] = 0;

        while (openList.Count > 0)
        {
            TileInfo current = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (costSoFar[openList[i]] + Heuristic(openList[i], target) <
                    costSoFar[current] + Heuristic(current, target))
                {
                    current = openList[i];
                }
            }

            if (current == target)
            {
                break;
            }

            openList.Remove(current);
            closedList.Add(current);

            foreach (var direction in directions)
            {
                TileInfo neighbor = GetNeighbor(current, direction);
                if (neighbor == null || neighbor.isObstacle || closedList.Contains(neighbor))
                {
                    continue;
                }

                float newCost = costSoFar[current] + Vector3.Distance(current.transform.position, neighbor.transform.position);
                if (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor])
                {
                    costSoFar[neighbor] = newCost;
                    float priority = newCost + Heuristic(neighbor, target);
                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                    cameFrom[neighbor] = current;
                }
            }
        }

        List<Vector3> path = new List<Vector3>();
        TileInfo temp = target;
        while (temp != null)
        {
            path.Add(temp.transform.position);
            cameFrom.TryGetValue(temp, out temp);
        }
        path.Reverse();

        return path;
    }

    private static float Heuristic(TileInfo a, TileInfo b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private static TileInfo GetNeighbor(TileInfo tile, Vector2Int direction)
    {
        RaycastHit hit;
        Vector3 checkPosition = tile.transform.position + new Vector3(direction.x, 0, direction.y);
        if (Physics.Raycast(checkPosition + Vector3.up * 2, Vector3.down, out hit))
        {
            return hit.collider.GetComponent<TileInfo>();
        }
        return null;
    }
}

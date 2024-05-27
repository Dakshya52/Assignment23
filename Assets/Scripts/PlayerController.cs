using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 2.0f; // Speed of the player
    public float gridSize = 1.0f; // Size of each grid step
    private bool isMoving = false; // Flag to check if the player is moving
    public EnemyAI enemy; // Reference to the enemy

    void Update()
    {
        if (!isMoving)
        {
            Vector3 direction = Vector3.zero;

            if (Input.GetKeyDown(KeyCode.W))
            {
                direction = Vector3.forward;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                direction = Vector3.back;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                direction = Vector3.left;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                direction = Vector3.right;
            }

            if (direction != Vector3.zero)
            {
                Vector3 targetPosition = transform.position + direction * gridSize;
                if (CanMoveTo(targetPosition))
                {
                    Debug.Log($"Moving to {targetPosition}");
                    StartCoroutine(MoveToPosition(targetPosition));
                }
                else
                {
                    Debug.Log($"Cannot move to {targetPosition} - Obstacle or invalid position");
                }
            }
        }
    }

    private bool CanMoveTo(Vector3 targetPosition)
    {
        RaycastHit hit;
        if (Physics.Raycast(targetPosition + Vector3.up * 2, Vector3.down, out hit))
        {
            TileInfo tileInfo = hit.collider.GetComponent<TileInfo>();
            if (tileInfo != null)
            {
                Debug.Log($"Target position {targetPosition} is a tile. Obstacle: {tileInfo.isObstacle}");
                return !tileInfo.isObstacle;
            }
            else
            {
                Debug.Log($"Target position {targetPosition} has no TileInfo component.");
            }
        }
        else
        {
            Debug.Log($"Raycast did not hit any collider at {targetPosition}");
        }
        return false;
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        isMoving = true;
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition; // Ensure exact final position
        isMoving = false;
        enemy.MoveTowardsPlayer(); // Notify the enemy to move
    }
}

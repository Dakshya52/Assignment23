using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2.0f; // Speed of the enemy
    public float gridSize = 1.0f; // Size of each grid step
    private bool isMoving = false; // Flag to check if the enemy is moving
    public Transform player; // Reference to the player

    public void MoveTowardsPlayer()
    {
        if (isMoving) return;

        Vector3 bestDirection = Vector3.zero;
        float shortestDistance = float.MaxValue;

        // Check all four possible directions
        Vector3[] possibleDirections = { Vector3.right, Vector3.left, Vector3.forward, Vector3.back };
        foreach (Vector3 direction in possibleDirections)
        {
            Vector3 possibleTargetPosition = transform.position + direction * gridSize;
            float distanceToPlayer = Vector3.Distance(possibleTargetPosition, player.position);

            if (CanMoveTo(possibleTargetPosition) && distanceToPlayer < shortestDistance)
            {
                shortestDistance = distanceToPlayer;
                bestDirection = direction;
            }
        }

        // Move to the best position
        if (bestDirection != Vector3.zero)
        {
            Vector3 targetPosition = transform.position + bestDirection * gridSize;
            StartCoroutine(MoveToPosition(targetPosition));
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
                return !tileInfo.isObstacle;
            }
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
    }
}

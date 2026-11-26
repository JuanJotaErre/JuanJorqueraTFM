using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class Movement : MonoBehaviour
{

    [SerializeField]
    readonly float speed = 5f;
    float actualSpeed = 5f;
    [SerializeField]
    int pushForce = 2;
    Vector3Int targetPosition;

    [SerializeField]
    Tilemap tilemap;

    [SerializeField]
    UnityEvent stop;


    

    public bool Move(Vector2Int movement)
    {

        var nextPosition = Vector3Int.FloorToInt(transform.position + this.BuildVector(movement));
        var free = CheckPositionIsFree(nextPosition);
        if(free)
        {
            targetPosition = nextPosition;
        }
        return free;
    }  

    private bool CheckPositionIsFree(Vector3Int nextPosition)
    {
        return !tilemap.HasTile(nextPosition);
    }

    public void MoveToTarget()
    {
        var currentPosition = transform.position;

        transform.position = Vector3.MoveTowards(currentPosition, targetPosition, Time.deltaTime * actualSpeed);
        if (transform.position == targetPosition)
        {
            actualSpeed = speed;
            stop.Invoke();
        }
    }

    Vector3Int BuildVector(Vector2Int vector)
    {
        return new Vector3Int(vector.x, vector.y, 0);
    }

    internal void PushTowards(Vector3Int direction)
    {

        if (direction == Vector3Int.zero)
            return;

        Vector3Int selectedDirection = direction;

        if (!CheckPositionIsFree(targetPosition + direction))
        {
            Vector3Int perpendicular1 =
                new Vector3Int(-direction.y, direction.x, 0);

            Vector3Int perpendicular2 =
                new Vector3Int(direction.y, -direction.x, 0);

            if (CheckPositionIsFree(targetPosition + perpendicular1))
            {
                selectedDirection = perpendicular1;
            }
            else if (CheckPositionIsFree(targetPosition + perpendicular2))
            {
                selectedDirection = perpendicular2;
            }
            else
            {
                return;
            }
        }

        Vector3Int destination = targetPosition;

        for (int i = 0; i < pushForce; i++)
        {
            Vector3Int next = destination + selectedDirection;

            if (!CheckPositionIsFree(next))
                break;

            destination = next;
        }

        if (destination == targetPosition)
            return;

        targetPosition = destination;
        actualSpeed = speed * 3;
    }
}

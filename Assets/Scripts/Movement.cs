using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class Movement : MonoBehaviour
{

    [SerializeField]
    float speed = 5f;
    Vector3 targetPosition;

    [SerializeField]
    Tilemap tilemap;

    [SerializeField]
    UnityEvent stop;


    

    public bool Move(Vector2 movement)
    {

        var nextPosition = transform.position + this.buildVector(movement);
        var free = CheckPositionIsFree(nextPosition);
        if(free)
        {
            targetPosition = nextPosition;
        }
        return free;
    }  

    private bool CheckPositionIsFree(Vector3 nextPosition)
    {
        return !tilemap.HasTile(Vector3Int.FloorToInt(nextPosition));
    }

    public void MoveToTarget()
    {
        var currentPosition = transform.position;

        transform.position = Vector3.MoveTowards(currentPosition, targetPosition, Time.deltaTime * speed);
        if (currentPosition == targetPosition)
        {
            stop.Invoke();
        }
    }

    Vector3 buildVector(Vector2 vector)
    {
        return new Vector3(vector.x, vector.y, 0);
    }
}

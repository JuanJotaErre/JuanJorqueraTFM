using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyController : MonoBehaviour
{


    [SerializeField]
    EnemyState state;
    [SerializeField]
    ActionState actionState;
    [SerializeField]
    float speed = 5f;
    [SerializeField]
    float sightDistance = 5f;
    [SerializeField]
    float minPause = .5f;
    [SerializeField]
    float maxPause = 1.5f;
    float pause;
    [SerializeField]
    PlayerController target;
    
    AStarPathFinding pathFinding;

    List<Node> path;
    Vector3Int lastPathCalculated;

    Vector3 nextMovePosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = EnemyState.PATROL;
        actionState = ActionState.STANDING;
        nextMovePosition = transform.position;

        pathFinding = GetComponentInParent<AStarPathFinding>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(state.Equals(EnemyState.PATROL))
        {
            Patrol();
        }
        else if(state.Equals(EnemyState.CHASE) && target != null)
        {
            CheckTargetDistanceAndChase();
        }
    }

    private void Patrol()
    {
        if(actionState.Equals(ActionState.STANDING))
        {
           actionState = ActionState.MOVING;
           nextMovePosition = transform.position + GetRandomDirection();
        }
        else if(actionState.Equals(ActionState.PAUSE))
        {
            pause = pause - Time.deltaTime;
            if(pause <= 0)
            {
                actionState = ActionState.STANDING;
            }
            CheckTarget();
        }
        else if(actionState.Equals(ActionState.MOVING))
        {
            MoveToTarget();
        }
    }

    private void CheckTarget()
    {
        if(Vector3.Distance(target.lastValidPosition, transform.position) <= sightDistance)
        {
            state = EnemyState.CHASE;
            actionState = ActionState.STANDING;
        }
    }

    private Vector3 GetRandomDirection()
    {
        return UnityEngine.Random.Range(0, 4) switch
        {
            0 =>  Vector3.up,
            1 =>  Vector3.right,
            2 =>  Vector3.down,
            3 =>  Vector3.left,
            _=> new Vector3(0,0,0)
        };
    }

    private void Chase()
    {

        if(actionState.Equals(ActionState.STANDING))
        {
            if(lastPathCalculated == null ||  lastPathCalculated != target.lastValidPosition) 
            {
                CalculateNewPath();
            }
            if(path != null && path.Count > 0)
            {
                nextMovePosition = path.First().GetPosition();
                path.RemoveAt(0);
                actionState = ActionState.MOVING;
            }
        }
        else if(actionState.Equals(ActionState.MOVING))
        {
            MoveToTarget();
        }
    }

    private void CheckTargetDistanceAndChase()
    {
        if(Vector3.Distance(target.lastValidPosition, transform.position) > sightDistance)
        {
            state = EnemyState.PATROL;
        } else
        {
            Chase();
        }
    }

    private void CalculateNewPath()
    {
        print("new path");
        path = pathFinding.GeneratePath(Vector3Int.FloorToInt(transform.position), Vector3Int.FloorToInt(target.lastValidPosition));
        lastPathCalculated = target.lastValidPosition;
    }

    void MoveToTarget()
    {
        var currentPosition = transform.position;

        transform.position = Vector3.MoveTowards(currentPosition, nextMovePosition, Time.deltaTime * speed);
        if (currentPosition == nextMovePosition && state == EnemyState.PATROL)
        {
            actionState = ActionState.PAUSE;
            pause = UnityEngine.Random.Range(minPause, maxPause);
        } else if (currentPosition == nextMovePosition)
        {
            actionState = ActionState.STANDING;
        }
    }
}

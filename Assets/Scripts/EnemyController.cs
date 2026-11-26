using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal.Internal;

public class EnemyController : MonoBehaviour
{


    [SerializeField]
    EnemyState state;
    [SerializeField]
    ActionState actionState;
    [SerializeField]
    float speed = 5f;
    [SerializeField]
    int strength = 1;
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

    Vector3Int nextMovePosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = EnemyState.PATROL;
        actionState = ActionState.STANDING;
        nextMovePosition = Vector3Int.FloorToInt(transform.position);

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
           nextMovePosition = Vector3Int.FloorToInt(transform.position + VectorUtil.GetRandomDirection());
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



    void OnTriggerEnter2D(Collider2D collider)
    {
        Collide(collider);
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        print("colliding");
        Collide(collider);
    }

    void Collide(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            collider.GetComponent<PlayerController>().OnDamage(strength, gameObject);
        }
    }
}

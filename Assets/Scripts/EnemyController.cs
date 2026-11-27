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

    Movement movement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = EnemyState.PATROL;
        actionState = ActionState.STANDING;

        pathFinding = GetComponentInParent<AStarPathFinding>();
        movement = GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state.Equals(EnemyState.PATROL))
            CheckTarget();
        if(state.Equals(EnemyState.PATROL))
        {
            Patrol();
        }
        else if(state.Equals(EnemyState.CHASE) && target != null)
        {
            CheckTargetDistanceAndChase();
        }

        if(actionState.Equals(ActionState.MOVING))
            movement.MoveToTarget();
    }

    private void Patrol()
    {
        if(actionState.Equals(ActionState.STANDING))
        {
            var moved = movement.Move(VectorUtil.GetRandomDirection2D());
            if(moved) 
                actionState = ActionState.MOVING;
        }
        else if(actionState.Equals(ActionState.PAUSE))
        {
            pause = pause - Time.deltaTime;
            if(pause <= 0)
            {
                actionState = ActionState.STANDING;
            }
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
            if(lastPathCalculated != target.lastValidPosition) 
            {
                CalculateNewPath();
            }
            if(path != null && path.Count > 0)
            {
                var moved = movement.MoveTo(path.First().GetPosition());
                if(moved) {
                    path.RemoveAt(0);
                    actionState = ActionState.MOVING;
                } else
                {
                    path.RemoveAll(n => true);
                }
            }
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


    public void OnStop()
    {
        if (state == EnemyState.PATROL)
        {
            actionState = ActionState.PAUSE;
            pause = UnityEngine.Random.Range(minPause, maxPause);
        } else
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
            collider.GetComponent<HealthController>().Damage(strength, gameObject);
            collider.GetComponent<PlayerController>().Push(gameObject);
        }
    }
}

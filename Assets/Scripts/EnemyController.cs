using System;
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
    float minPause = .5f;
    [SerializeField]
    float maxPause = 1.5f;
    float pause;
    Vector3 targetPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = EnemyState.PATROL;
        actionState = ActionState.STANDING;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(state.Equals(EnemyState.PATROL))
        {
            patrol();
        }
        else if(state.Equals(EnemyState.CHASE))
        {
            chase();
        }
    }

    private void patrol()
    {
        if(actionState.Equals(ActionState.STANDING))
        {
           actionState = ActionState.MOVING;
           targetPosition = transform.position + getRandomDirection();
        }
        if(actionState.Equals(ActionState.PAUSE))
        {
            pause = pause - Time.deltaTime;
            if(pause <= 0)
            {
                actionState = ActionState.STANDING;
            }
        }
        else if(actionState.Equals(ActionState.MOVING))
        {
            moveToTarget();
        }
    }

    private Vector3 getRandomDirection()
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

    private void chase()
    {
        throw new NotImplementedException();
    }

    void moveToTarget()
    {
        var currentPosition = transform.position;

        transform.position = Vector3.MoveTowards(currentPosition, targetPosition, Time.deltaTime * speed);
        if (currentPosition == targetPosition)
        {
            actionState = ActionState.PAUSE;
            pause = UnityEngine.Random.Range(minPause, maxPause);
        }
    }
}

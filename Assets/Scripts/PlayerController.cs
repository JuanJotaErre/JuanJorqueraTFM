using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    InputAction moveAction;
    [SerializeField]
    PlayerState state;
    [SerializeField]
    float speed = 5f;
    Vector3 targetPosition;
    
    void Start()
    {
        state = PlayerState.STANDING;
        moveAction = InputSystem.actions.FindAction("Move");
        transform.position = new Vector3(0,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        if(state.Equals(PlayerState.STANDING) && moveAction.IsPressed())
        {
            var moveValue = GetMoveValue();
            targetPosition = transform.position + this.buildVector(moveValue);
            state = PlayerState.MOVING;
        }
        else if(state.Equals(PlayerState.MOVING))
        {
            moveToTarget();
        }
    }

    private Vector3 GetMoveValue()
    {
        var rawValue = moveAction.ReadValue<Vector2>();
        var x = rawValue.x;
        var y = rawValue.y;
        var vector = new Vector3();
        if(Math.Abs(x) >= Math.Abs(y))
        {
            vector.x = x;
        } else
        {
            vector.y = y;
        }
        return vector.normalized;
    }

    void moveToTarget()
    {
        var currentPosition = transform.position;

        transform.position = Vector3.MoveTowards(currentPosition, targetPosition, Time.deltaTime * speed);
        if (currentPosition == targetPosition)
        {
            state = PlayerState.STANDING;
        }
    }

    Vector3 buildVector(Vector2 vector)
    {
        return new Vector3(vector.x, vector.y, 0);
    }
}

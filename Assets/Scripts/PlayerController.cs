using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{

    InputAction moveAction;
    
    [SerializeField]
    ActionState state;

    [SerializeField]
    float speed = 5f;
    Vector3 targetPosition;

    [SerializeField]
    Tilemap tilemap;
    
    void Start()
    {
        state = ActionState.STANDING;
        moveAction = InputSystem.actions.FindAction("Move");
        transform.position = new Vector3(0,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        if(state.Equals(ActionState.STANDING) && moveAction.IsPressed())
        {
            var moveValue = GetMoveValue();
            var nextPosition = transform.position + this.buildVector(moveValue);
            if(CheckPositionIsFree(nextPosition))
            {
                targetPosition = nextPosition;
                state = ActionState.MOVING;
            }
        }
        else if(state.Equals(ActionState.MOVING))
        {
            moveToTarget();
        }
    }

    private bool CheckPositionIsFree(Vector3 nextPosition)
    {
        return !tilemap.HasTile(Vector3Int.FloorToInt(nextPosition));
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
            state = ActionState.STANDING;
        }
    }

    Vector3 buildVector(Vector2 vector)
    {
        return new Vector3(vector.x, vector.y, 0);
    }
}

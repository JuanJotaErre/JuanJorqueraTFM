using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{

    InputAction moveAction;
    
    [SerializeField]
    ActionState state;

    // [SerializeField]
    // float speed = 5f;
    Vector3 targetPosition;

    [SerializeField]
    Movement movement;

    [SerializeField]
    Tilemap tilemap;

    bool lastMovementAxisX = false;

    public Vector3Int lastValidPosition;
    
    void Start()
    {
        state = ActionState.STANDING;
        moveAction = InputSystem.actions.FindAction("Move");
        transform.position = new Vector3(0,0,0);
        lastValidPosition = Vector3Int.FloorToInt(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if(state.Equals(ActionState.STANDING) && moveAction.IsPressed())
        {
            var moveValue = GetMoveValue();
            if (moveValue.magnitude > 0) {
                var moved = movement.Move(moveValue);
                if(moved)
                {
                    state = ActionState.MOVING;
                }
            }
        } else if( state.Equals(ActionState.MOVING))
        {
            movement.MoveToTarget();
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
        if(Math.Abs(x) == Math.Abs(y))
        {
            if(lastMovementAxisX)
            {
                vector.y = y;
            } else
            {
                vector.x = x;
            }
        }
        else if(Math.Abs(x) > Math.Abs(y))
        {
            vector.x = x;
            lastMovementAxisX = true;
        } else
        {
            vector.y = y;
            lastMovementAxisX = false;
        }
        return vector.normalized;
    }

    public void OnStop()
    {
        state = ActionState.STANDING;

        lastValidPosition = Vector3Int.FloorToInt(transform.position);
    }
}

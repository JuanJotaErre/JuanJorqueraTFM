using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{

    InputAction moveAction;
    InputAction AttackAction;
    
    [SerializeField]
    ActionState state;

    [SerializeField]
    Movement movement;

    [SerializeField]
    Tilemap tilemap;

    bool lastMovementAxisX = false;
    Vector2Int facing;

    public Vector3Int lastValidPosition;

    public Collider2D collider;

    public AttackController weapon;

    


    void Start()
    {
        state = ActionState.STANDING;
        moveAction = InputSystem.actions.FindAction("Move");
        AttackAction = InputSystem.actions.FindAction("Attack");
        transform.position = new Vector3Int(0,0,0);
        lastValidPosition = Vector3Int.FloorToInt(transform.position);
        weapon = GetComponentInChildren<AttackController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state.Equals(ActionState.STANDING) && moveAction.IsPressed())
        {
            var moveValue = GetMoveValue();
            if (moveValue.magnitude > 0) {
                facing = moveValue;
                var moved = movement.Move(moveValue);
                if(moved)
                {
                    state = ActionState.MOVING;
                }
            }
        } else if(state.Equals(ActionState.MOVING))
        {
            movement.MoveToTarget();
        }
        CheckAttack();
    }

    private void CheckAttack()
    {
        if(AttackAction.IsPressed())
        {
            weapon.Attack(facing);
        }
    }

    private Vector2Int GetMoveValue()
    {
        var rawValue = moveAction.ReadValue<Vector2>();
        
        var x = rawValue.x;
        var y = rawValue.y;
        var vector = new Vector2();
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
        return Vector2Int.FloorToInt(vector.normalized);
    }

    public void OnStop()
    {
        state = ActionState.STANDING;

        lastValidPosition = Vector3Int.FloorToInt(transform.position);
    }

    public void Push(GameObject collider)
    {
        var direction = Vector3Int.FloorToInt((transform.position - collider.transform.position).normalized);
        if(direction.magnitude < 1)
        {
            direction = VectorUtil.GetRandomDirection();
        }
        movement.PushTowards(direction);
        state = ActionState.MOVING;
    }
}

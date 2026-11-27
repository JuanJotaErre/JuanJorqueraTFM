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
    int health = 10;

    [SerializeField]
    Movement movement;

    [SerializeField]
    Tilemap tilemap;

    bool lastMovementAxisX = false;

    public Vector3Int lastValidPosition;

    public Collider2D collider;

    float lastDamage = 0;
    float invicibilitySeconds = 1f;


    void Start()
    {
        state = ActionState.STANDING;
        moveAction = InputSystem.actions.FindAction("Move");
        transform.position = new Vector3Int(0,0,0);
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


    public void OnDamage(int strength, GameObject collider)
    {
        if(Time.time - lastDamage > invicibilitySeconds)
        {
            health -= strength;
            lastDamage = Time.time;
            Push(collider);
        }
    }

    private void Push(GameObject collider)
    {
        var direction = Vector3Int.FloorToInt((transform.position - collider.transform.position).normalized);
        if(direction.magnitude < 1)
        {
            direction = VectorUtil.GetRandomDirection();
        }
        movement.PushTowards(direction);
        state = ActionState.MOVING;
    }

    internal void Heal(int healStrength)
    {
        health += healStrength;
    }
}

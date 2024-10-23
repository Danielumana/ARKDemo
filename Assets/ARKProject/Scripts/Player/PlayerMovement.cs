using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    
    PlayerInput playerInput;
    InputAction moveAction;

    void Start()
    {
        
        bool inputSetupSuccess = TryPlayerInputSetup();

    }

    
    void Update()
    {
        Move();
    }

    bool TryPlayerInputSetup()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            return false;
        }

        moveAction = playerInput.actions.FindAction("MoveAction");
        if (moveAction == null)
        {
           return false; 
        }
        return true;
    }

    void Move()
    {
        if (moveAction == null)
        {
            return;
        }
        Vector2 direction = moveAction.ReadValue<Vector2>();
        transform.position += new Vector3(direction.x, 0, direction.y) * Time.deltaTime;
    }

}

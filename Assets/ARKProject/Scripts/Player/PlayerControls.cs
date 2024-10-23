using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    //Player Controller Components
    private PlayerInput playerInput;
    private InputAction moveAction;


    //Ball References(Change placeholder)
    public GameObject ballReference;
    private Rigidbody ballRigidbody;

    void Start()
    {
        TryPlayerInputSetup();
        ballRigidbody = ballReference.GetComponent<Rigidbody>();
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

    public void InitialImpulse(InputAction.CallbackContext callbackContext)
    {
        ballRigidbody.linearVelocity = new Vector3(Random.Range(-1.0f,1.0f), 1, 0).normalized * 5.0f;
    }
}

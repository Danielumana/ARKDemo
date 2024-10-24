using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    //Player Controller Components
    private PlayerInput playerInput;
    private InputAction moveAction;

bool done = false;
    //Ball References(Change placeholder)
    public GameObject ballReference;
    private Rigidbody ballRigidbody;

    //Movement Values
    public float movementSpeedMultiplier = 10.0f;
    public float intialImpulseSpeedMultiplier = 5.0f;

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
        transform.position += new Vector3(direction.x, 0, direction.y) * Time.deltaTime * movementSpeedMultiplier;
    }

    public void InitialImpulse(InputAction.CallbackContext callbackContext)
    {
        if (done == true)
        {
            return;
        }
        done = true;
        float random = Random.Range(0.4f,0.8f);
        ballRigidbody.linearVelocity = new Vector3(random, 1, 0).normalized * intialImpulseSpeedMultiplier;
    }
}

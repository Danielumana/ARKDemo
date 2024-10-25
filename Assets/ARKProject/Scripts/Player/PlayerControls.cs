using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{

    //Player Controller Components
    private PlayerInput playerInput;
    private InputAction moveAction;

    bool bIntialImpulseActionDone = false;
    //Ball References(Change placeholder)
    public GameObject ballReference;


    //Movement Values
    public float movementSpeedMultiplier = 10.0f;
    public float intialImpulseSpeedMultiplier = 5.0f;

    void Start()
    {
        TryPlayerInputSetup();
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
        if (bIntialImpulseActionDone == true)
        {
            return;
        }
        bIntialImpulseActionDone = true;
        if (ballReference == null)
        {
            return;
        }
        BallMovement ballMovementClass = ballReference.GetComponent<BallMovement>();
        if (ballMovementClass == null)
        {
            return;
        }
        float randomImpulseX = Random.Range(0.4f,0.8f);
        Vector3 intialImpulseDirection = new Vector3(randomImpulseX, 1, 0).normalized;
        ballMovementClass.OnInitialImpulseAction(intialImpulseDirection);
        // initialImpulseActionDelegate(new Vector3(randomImpulseX, 1, 0).normalized * intialImpulseSpeedMultiplier);
    }
}

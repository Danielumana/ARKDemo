using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{

    public delegate void OnInitialImpulseAction(Vector3 initialImpulseDirection);
    public static event OnInitialImpulseAction InitialImpulseAction;

    //Player Controller Components
    private PlayerInput playerInput;
    private InputAction moveAction;

    bool bIntialImpulseActionDone = false;

    Dictionary<string, bool> availableControlsState = new Dictionary<string, bool>{
        
        {"InitialImpulse", false},
        {"Move",false}
    
    };

    //Ball References
    GameObject initialBallReference;

    //Movement Values
    public float movementSpeedMultiplier = 10.0f;
    public float intialImpulseSpeedMultiplier = 5.0f;

    void OnEnable() 
    {
        ARKGameMode.GameStateChanged += OnGameStateChanged; 
    }

    void OnDisable() 
    {
        ARKGameMode.GameStateChanged -= OnGameStateChanged;
    }

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

    // public void set enable
    // public void SetEnableAllControls()
    // {

    // }

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
        if (initialBallReference == null)
        {
            initialBallReference = ARKGameMode.Instance.GetMainBallReference();
            if (initialBallReference == null)
            {
                return;
            }
        }
        BallMovement ballMovementClass = initialBallReference.GetComponent<BallMovement>();
        if (ballMovementClass == null)
        {
            return;
        }
        float randomImpulseX = Random.Range(0.4f,0.8f);
        Vector3 intialImpulseDir = new Vector3(randomImpulseX, 1, 0).normalized;
        InitialImpulseAction(intialImpulseDir);
    }

    public bool GetIntialImpulseDone()
    {
        return bIntialImpulseActionDone;
    }

    private void OnGameStateChanged(ARKGameMode.GameState newState, ARKGameMode.GameState oldState)
    {
        switch (newState)
        {
            case ARKGameMode.GameState.InitialBall:
                bIntialImpulseActionDone = false;
            break;
        }
    }

}

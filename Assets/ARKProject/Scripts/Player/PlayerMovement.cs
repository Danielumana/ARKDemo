using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{

    public delegate void OnInitialImpulseAction(Vector3 initialImpulseDirection);
    public static event OnInitialImpulseAction InitialImpulseAction;

    public delegate void OnPauseAction();
    public static event OnPauseAction PauseAction;

    //Player Controller Components
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction pauseAction;

    bool bIntialImpulseActionDone = false;

    Dictionary<string, bool> availableControlsState = new Dictionary<string, bool>{
        
        {"InitialImpulse", false},
        {"Move", false},
        {"Pause", false}
    
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

    void TryPlayerInputSetup()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            return;
        }
        moveAction = playerInput.actions.FindAction("MoveAction");
        pauseAction = playerInput.actions.FindAction("PauseAction");
        SetAllControlsEnableValue(true);
        return;
    }

    public void SetControlEnableValue(String controlKey, bool newState)
    {
        if (availableControlsState.ContainsKey(controlKey) == false)
        {
            return;
        }
        availableControlsState[controlKey] = newState;
    }
    public void SetAllControlsEnableValue(bool newState)
    {
        Dictionary<string, bool> availableControlsStateCopy = new Dictionary<string, bool>(availableControlsState);
        foreach (string controlKey in availableControlsStateCopy.Keys)
        {
            availableControlsState[controlKey] = newState;
        }
    }

    public bool GetControlEnableValue(string controlKey)
    {
        if (availableControlsState.ContainsKey(controlKey) == false)
        {
            return false;
        }
        return availableControlsState[controlKey];

    }

    void Move()
    {
        if (GetControlEnableValue("Move") == false)
        {
            return;
        }
        if (moveAction == null)
        {
            return;
        }
        Vector2 direction = moveAction.ReadValue<Vector2>();
        transform.position += new Vector3(direction.x, 0, direction.y) * Time.deltaTime * movementSpeedMultiplier;
    }

    public void InitialImpulse(InputAction.CallbackContext callbackContext)
    {
        if (GetControlEnableValue("InitialImpulse") == false)
        {
            return;
        }
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
        float randomImpulseX = UnityEngine.Random.Range(0.4f,0.8f);
        Vector3 intialImpulseDir = new Vector3(randomImpulseX, 1, 0).normalized;
        InitialImpulseAction(intialImpulseDir);
    }

    public void Pause(InputAction.CallbackContext callbackContext)
    {
        if (GetControlEnableValue("Pause") == false)
        {
            return;
        }
        PauseAction();
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
                SetAllControlsEnableValue(true);
                break;

            case ARKGameMode.GameState.Playing:
                SetControlEnableValue("InitialImpulse", false);
                break;
            case ARKGameMode.GameState.GameWin:
                SetAllControlsEnableValue(false);
                break;
            case ARKGameMode.GameState.GameLost:
                SetAllControlsEnableValue(false);
                break;
        }
    }

}

using UnityEngine;

public class BallMovement : MonoBehaviour
{

    GameObject paddleReference;
    private Vector3 currentMovementDirection;
    public float desiredConstantSpeed = 10.0f;

    private bool bIntialImpulseDone = false;

    void OnEnable() 
    {
        PlayerMovement.InitialImpulseAction += OnInitialImpulseAction;
        ARKGameMode.GameStateChanged += OnGameStateChanged; 
    }
    void OnDisable() 
    {
        PlayerMovement.InitialImpulseAction -= OnInitialImpulseAction;
        ARKGameMode.GameStateChanged -= OnGameStateChanged;
    }
    void Start()
    {
        paddleReference = GameObject.Find("Paddle");
    }

    void Update()
    {
        ARKGameMode gameModeReference = ARKGameMode.Instance;
        if (gameModeReference == null)
        {
            return;
        }
        ARKGameMode.GameState currentState = gameModeReference.GetCurrentGameState();
        if (bIntialImpulseDone == false && currentState == ARKGameMode.GameState.InitialBall)
        {
            transform.position = new Vector3(paddleReference.transform.position.x, transform.position.y,transform.position.z);
        }
        else if (currentState == ARKGameMode.GameState.Playing)
        {
            transform.position += currentMovementDirection * desiredConstantSpeed * Time.deltaTime;   
        }
    }

    private void OnInitialImpulseAction(Vector3 initialImpulseDirection)
    {
        if (ARKGameMode.Instance != null)
        {
            if (ARKGameMode.Instance.GetMainBallReference() != this.gameObject)
            {
                ARKGameMode.GameStateChanged -= OnGameStateChanged;
                return;
            }
        }
        if (bIntialImpulseDone == true)
        {
            return;
        }
        bIntialImpulseDone = true;
        currentMovementDirection = initialImpulseDirection; 
    }

    private void OnCollisionEnter(Collision other) 
    {
        Vector3 collisionContactPointNormal = other.GetContact(0).normal;
        Vector3 reflectedBounceDirection = Vector3.Reflect(currentMovementDirection.normalized, collisionContactPointNormal);

        if (Mathf.Abs(reflectedBounceDirection.x) < 0.1f)
        {
            reflectedBounceDirection.x = Mathf.Sign(reflectedBounceDirection.x) * 0.3f;
        }
        if (Mathf.Abs(reflectedBounceDirection.y) < 0.1f)
        {
            reflectedBounceDirection.y = Mathf.Sign(reflectedBounceDirection.y) * 0.3f;
        }
        currentMovementDirection = reflectedBounceDirection.normalized;
    }

    private void OnGameStateChanged(ARKGameMode.GameState newState, ARKGameMode.GameState oldState)
    {
        switch (newState)
        {
            case ARKGameMode.GameState.InitialBall:
                bIntialImpulseDone = false;
                currentMovementDirection = Vector3.zero;
            break;
        }
    }

}

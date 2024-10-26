using UnityEngine;

public class BallMovement : MonoBehaviour
{
    GameObject paddleReference;
    private Vector3 currentMovementDirection;
    public float desiredConstantSpeed = 10.0f;

    private bool bIntialImpulseDone = false;

    void OnEnable() 
    {
        ARKGameMode gameModeReference = ARKGameMode.Instance;
        if ( gameModeReference == null)
        {
            return;
        }
        ARKGameMode.GameStateChanged += OnGameStateChanged; 
    }
    void OnDisable() 
    {
        ARKGameMode.GameStateChanged -= OnGameStateChanged;
    }
    void Start()
    {
        paddleReference = GameObject.Find("Paddle");
    }

    void Update()
    {
        if (bIntialImpulseDone == false)
        {
            return;
        }
        transform.position += currentMovementDirection * desiredConstantSpeed * Time.deltaTime;
    }

    public void OnInitialImpulseAction(Vector3 initialImpulseDirection)
    {
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

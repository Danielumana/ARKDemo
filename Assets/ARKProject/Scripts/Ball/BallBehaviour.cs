using UnityEngine;

public class BallMovement : MonoBehaviour
{

    private GameObject ballReference;
    private Rigidbody ballRigidbody;

    public float desiredConstantSpeed = 10.0f;
    void Start()
    {
        ballReference = this.gameObject;
        if (ballReference == null)
        {
            return;
        }
        ballRigidbody = ballReference.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (ballRigidbody == null)
        {
            return;
        }
        Debug.Log("Actual Velocity: "+ballRigidbody.linearVelocity);
    }

    private void OnCollisionEnter(Collision otherCollision) 
    {
        if (ballRigidbody == null)
        {
            return;
        }

        Vector3 currentBallNormalizedVelocity = ballRigidbody.linearVelocity.normalized;
        ballRigidbody.linearVelocity = currentBallNormalizedVelocity * desiredConstantSpeed;

    }

}

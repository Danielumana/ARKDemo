using Unity.Mathematics;
using UnityEngine;

public class BallMovement : MonoBehaviour
{

    private GameObject ballReference;
    private Rigidbody ballRigidbody;

    public float desiredConstantSpeed = 2.0f;

    private float lastCollisionTime = 0.0f;
    private float minimumTime = 0.6f;

    private Vector3 lastValidBounceDirection;

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
        
    }


    private void OnCollisionEnter(Collision otherCollision) 
    {
        // if (ballRigidbody == null)
        // {
        //     return;
        // }

        Vector3 currentBallNormalizedVelocity = ballRigidbody.linearVelocity.normalized;
        Vector3 collisionContactPointNormal = otherCollision.GetContact(0).normal;
        Vector3 reflectedBounceDirection = Vector3.Reflect(currentBallNormalizedVelocity, collisionContactPointNormal);


        float  lastBounceDotP = Vector3.Dot(collisionContactPointNormal, lastValidBounceDirection);

        float  bounceDotP = Vector3.Dot(collisionContactPointNormal, reflectedBounceDirection);

        if (bounceDotP <= -0.9f || bounceDotP >= 0.9f )
        {
            float rotationSign = 1;
            if (lastBounceDotP > -0.8f || lastBounceDotP < 0.8f )
            {
                if (Mathf.Abs(reflectedBounceDirection.x) <= 0.2f)
                {
                    rotationSign = Mathf.Sign(lastValidBounceDirection.x);
                }
                else if (Mathf.Abs(reflectedBounceDirection.y) < 0.2f)
                {
                    rotationSign = Mathf.Sign(lastValidBounceDirection.y);
                }
            }
            Quaternion rotationModification = Quaternion.AngleAxis(30.0f * rotationSign, Vector3.forward);
            lastValidBounceDirection = rotationModification * reflectedBounceDirection;
        }
        else
        {
            lastValidBounceDirection = reflectedBounceDirection;
        }
        
        lastCollisionTime = Time.time;
        ballRigidbody.linearVelocity = lastValidBounceDirection.normalized * desiredConstantSpeed;
    }

}

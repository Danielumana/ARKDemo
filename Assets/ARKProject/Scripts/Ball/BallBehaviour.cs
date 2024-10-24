using Unity.Mathematics;
using UnityEngine;

public class BallMovement : MonoBehaviour
{

    private GameObject ballReference;
    private Rigidbody ballRigidbody;
    private Vector3 lastValidBounceDirection;
    private float rotationModificationAngle = 30.0f;
    private float minReflectionValueXY = 0.2f;

    private float bounceDotPLimit = 0.85f;

    public float desiredConstantSpeed = 2.0f;

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
        if (ballRigidbody == null)
        {
            return;
        }

        Vector3 currentBallNormalizedVelocity = ballRigidbody.linearVelocity.normalized;
        Vector3 collisionContactPointNormal = otherCollision.GetContact(0).normal;
        Vector3 reflectedBounceDirection = Vector3.Reflect(currentBallNormalizedVelocity, collisionContactPointNormal);


        float  lastBounceDotP = Vector3.Dot(collisionContactPointNormal, lastValidBounceDirection);

        float  bounceDotP = Vector3.Dot(collisionContactPointNormal, reflectedBounceDirection);

        if (bounceDotP <= -bounceDotPLimit || bounceDotP >= bounceDotPLimit )
        {
            float rotationSign = 1;
            if (lastBounceDotP > -bounceDotPLimit || lastBounceDotP < bounceDotPLimit )
            {
                if (Mathf.Abs(reflectedBounceDirection.x) <= minReflectionValueXY)
                {
                    rotationSign = Mathf.Sign(lastValidBounceDirection.x);
                }
                else if (Mathf.Abs(reflectedBounceDirection.y) <= minReflectionValueXY)
                {
                    rotationSign = Mathf.Sign(lastValidBounceDirection.y);
                }
            }
            Quaternion rotationModification = Quaternion.AngleAxis(rotationModificationAngle * rotationSign, Vector3.forward);
            lastValidBounceDirection = rotationModification * reflectedBounceDirection;
        }
        else
        {
            lastValidBounceDirection = reflectedBounceDirection;
        }
        
        ballRigidbody.linearVelocity = lastValidBounceDirection.normalized * desiredConstantSpeed;
    }

}

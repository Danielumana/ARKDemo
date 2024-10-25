using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Vector3 currentMovementDirection;
    public float desiredConstantSpeed = 10.0f;

    private bool bIntialImpulseDone = false;

    void Start()
    {
        
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

    // private void OnCollisionEnter(Collision otherCollision) 
    // {
    //     if (ballRigidbody == null)
    //     {
    //         return;
    //     }

    //     if (Time.time - lastCollisionTime < minCollisionTime)
    //     {
    //         Debug.Log("Rapidooooo");
    //         return;
    //     }
    //     lastCollisionTime = Time.time;

    //     Vector3 currentBallNormalizedVelocity = ballRigidbody.linearVelocity.normalized;
    //     Vector3 collisionContactPointNormal = otherCollision.GetContact(0).normal;
    //     Vector3 reflectedBounceDirection = Vector3.Reflect(currentBallNormalizedVelocity, collisionContactPointNormal);

        
        

    //     float  lastBounceDotP = Vector3.Dot(collisionContactPointNormal, lastValidBounceDirection);

    //     float  bounceDotP = Vector3.Dot(collisionContactPointNormal, reflectedBounceDirection);

    //     //Debug.Log("last valid vector"+lastValidBounceDirection+" DotP = "+lastBounceDotP);
    //     // Debug.Log("currentBallNormalizedVelocity= "+currentBallNormalizedVelocity);
    //     // Debug.Log("reflection vector "+reflectedBounceDirection+"DotP = "+bounceDotP);
    //     // Debug.Log("Surface normal= "+collisionContactPointNormal);

    //     if (Mathf.Abs(reflectedBounceDirection.x) < 0.1f)
    //     {
    //         Debug.Log("X "+reflectedBounceDirection.x);

    //         reflectedBounceDirection.x = Mathf.Sign(reflectedBounceDirection.x) * 0.3f;
    //         Debug.Log("X2 "+reflectedBounceDirection.x);
    //     }
    //     if (Mathf.Abs(reflectedBounceDirection.y) < 0.1f)
    //     {
    //         Debug.Log("Y "+reflectedBounceDirection.y);
    //         reflectedBounceDirection.y = Mathf.Sign(reflectedBounceDirection.y) * 0.3f;
    //     }
    //     Vector3 newVector =reflectedBounceDirection.normalized*10;
    //     ballRigidbody.linearVelocity = reflectedBounceDirection.normalized * desiredConstantSpeed;
    //     lastValidBounceDirection = reflectedBounceDirection;
    //     // if (bounceDotP <= -bounceDotPLimit || bounceDotP >= bounceDotPLimit )
    //     // {
    //     //     float rotationSign = 1;
    //     //     if (lastBounceDotP > -bounceDotPLimit || lastBounceDotP < bounceDotPLimit )
    //     //     {
    //     //         if (Mathf.Abs(reflectedBounceDirection.x) <= minReflectionValueXY)
    //     //         {
    //     //             rotationSign = Mathf.Sign(reflectedBounceDirection.x);
    //     //         }
    //     //         else if (Mathf.Abs(reflectedBounceDirection.y) <= minReflectionValueXY)
    //     //         {
    //     //             rotationSign = Mathf.Sign(reflectedBounceDirection.y);
    //     //         }
    //     //     }
    //     //     Quaternion rotationModification = Quaternion.AngleAxis(rotationModificationAngle * rotationSign, Vector3.forward);
    //     //     lastValidBounceDirection = rotationModification * reflectedBounceDirection;
    //     // }
    //     // else
    //     // {
    //     //     lastValidBounceDirection = reflectedBounceDirection;
    //     // }
    //     // Debug.Log("Final reflection vector"+lastValidBounceDirection.normalized);
    //     // ballRigidbody.linearVelocity = lastValidBounceDirection.normalized * desiredConstantSpeed;
    // }

}

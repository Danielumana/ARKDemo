using UnityEngine;

public class BallMovement : MonoBehaviour
{

    private GameObject ballReference;
    private Rigidbody ballRigidbody;
    private Vector3 lastValidBounceDirection;
    private float rotationModificationAngle = 30.0f;
    private float minReflectionValueXY = 0.2f;

    private float bounceDotPLimit = 0.85f;

    public float desiredConstantSpeed = 10.0f;


    float lastCollisionTime;
    float minCollisionTime = 0.2f;

    void Start()
    {
        lastValidBounceDirection = new Vector3(0.5f,1.0f,0.0f);
        lastCollisionTime = Time.time;
        ballReference = this.gameObject;
        if (ballReference == null)
        {
            return;
        }
        ballRigidbody = ballReference.GetComponent<Rigidbody>();
    }

    void Update()
    {
        transform.position += lastValidBounceDirection * desiredConstantSpeed * Time.deltaTime;
        //Debug.Log("direction: "+lastValidBounceDirection);
    }

    private void OnCollisionEnter(Collision other) 
    {
  
        //Vector3 currentBallNormalizedVelocity = ballRigidbody.linearVelocity.normalized;
        Vector3 collisionContactPointNormal = other.GetContact(0).normal;
        Vector3 reflectedBounceDirection = Vector3.Reflect(lastValidBounceDirection.normalized, collisionContactPointNormal);

        if (Mathf.Abs(reflectedBounceDirection.x) < 0.1f)
        {
            reflectedBounceDirection.x = Mathf.Sign(reflectedBounceDirection.x) * 0.3f;
        }
        if (Mathf.Abs(reflectedBounceDirection.y) < 0.1f)
        {
            reflectedBounceDirection.y = Mathf.Sign(reflectedBounceDirection.y) * 0.3f;
        }

        lastValidBounceDirection = reflectedBounceDirection.normalized;
        //Debug.Log("colisión");

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


    void SetCollisionEnable(Collider collider, bool bEnable)
    {
        Physics.IgnoreCollision(ballReference.GetComponent<Collider>(), collider, bEnable);
    }

}

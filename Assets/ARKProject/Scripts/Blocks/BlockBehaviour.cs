using UnityEngine;

public class BlockBehaviour : MonoBehaviour
{
    
    private GameObject blockReference;

    public float blockHits = 1.0f;

    void Start()
    {
        blockReference = this.gameObject;
    }

    
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision otherCollision) 
    {
        if (blockReference == null)
        {
            return;
        }
        
        if (otherCollision == null)
        {
            return;
        }
        if (otherCollision.gameObject.tag != "Ball")
        {
            return;
        }
        
        blockHits -= 1;
        if (blockHits >= 1)
        {
            return;
        }

        Destroy(blockReference);

    }

}

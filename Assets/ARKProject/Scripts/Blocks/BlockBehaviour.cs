using UnityEngine;

public class BlockBehaviour : MonoBehaviour
{
    private GameObject blockReference;
    public int blockHits = 1;
    private int onDestructionScore;

    void Start()
    {
        blockReference = this.gameObject;
        onDestructionScore = (int)Mathf.Pow(blockHits,2);
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
        if (ARKGameMode.Instance == null)
        {
            Debug.Log("Nulllll");
            return;
        }
        ARKGameMode.Instance.AddPointsToScore(onDestructionScore);
        
    }

}

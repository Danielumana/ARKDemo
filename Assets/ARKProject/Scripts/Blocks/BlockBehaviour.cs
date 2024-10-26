using UnityEngine;

public class BlockBehaviour : MonoBehaviour
{

    public delegate void OnBlockDestruction(int onBlockDestructionPoins);
    public static event OnBlockDestruction OnBlockDestructionEvent;

    private GameObject blockReference;
    public int blockHits = 1;
    private int onDestructionPoints;

    void Start()
    {
        blockReference = this.gameObject;
        onDestructionPoints = (int)Mathf.Pow(blockHits,2);
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
            return;
        }
        OnBlockDestructionEvent(onDestructionPoints);
    }

}

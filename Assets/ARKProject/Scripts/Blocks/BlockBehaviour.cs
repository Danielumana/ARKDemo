using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BlockBehaviour : MonoBehaviour
{

    public delegate void OnBlockDestruction(int onBlockDestructionPoins, Vector3 blockPosition);
    public static event OnBlockDestruction OnBlockDestructionEvent;

    private GameObject blockReference;
    public int blockHits = 1;
    private int onDestructionPoints;

    List<Color> blockColors = new List<Color> 
    {
        Color.gray,
        Color.green,
        Color.yellow,
        Color.blue,
        Color.magenta,
        Color.red
    };

    void Start()
    {
        blockReference = this.gameObject;
        onDestructionPoints = (int)Mathf.Pow(blockHits,2);
        blockHits = math.clamp(blockHits, 0, 1000);
        MeshRenderer blockMeshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (blockMeshRenderer == null)
        {
            return;
        }
        blockMeshRenderer.material.color = blockHits < blockColors.Count ? blockColors[blockHits] : blockColors[blockColors.Count - 1];
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

        Vector3 blockPosition = blockReference.transform.position;
        Destroy(blockReference);
        OnBlockDestructionEvent(onDestructionPoints, blockPosition);
    }

}

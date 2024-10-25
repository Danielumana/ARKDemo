using UnityEngine;

public class DeathVolume : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    
    private void OnCollisionEnter(Collision otherCollision) 
    {
        GameObject otherGameObject = otherCollision.gameObject;
        if (otherGameObject == null)
        {
            return;
        }
        if (otherGameObject.tag != "Ball")
        {
            return;
        }
        ARKGameMode gameModeReference = ARKGameMode.Instance;
        if (gameModeReference == null)
        {
            return;
        }
        gameModeReference.DespawnBall(otherGameObject);
    }

}

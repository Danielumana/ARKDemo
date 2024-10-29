using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    
    List<GameObject> activeDroppedPowerUps = new List<GameObject>();

    List<String> powerUpKeysList = new List<string>
    {
        "MultipleBalls",
        "ExtraLive",
        "FasterBall"
    };

    public GameObject powerUpPrefabReference;

    void OnEnable() 
    {
        BlockBehaviour.OnBlockDestructionEvent += OnBlockDestruction;
    }

    void OnDisable() 
    {
        BlockBehaviour.OnBlockDestructionEvent -= OnBlockDestruction;
    }
    
    void Start()
    {
        
    }

   
    void Update()
    {
        if (activeDroppedPowerUps.Count < 0)
        {
            return;
        }
        List<GameObject> activeDroppedPowerUpsCopy = new List<GameObject>(activeDroppedPowerUps);
        foreach (GameObject droppedPowerUpObject in activeDroppedPowerUpsCopy)
        {
            if (droppedPowerUpObject == null)
            {
                
                activeDroppedPowerUps.Remove(droppedPowerUpObject);
                continue;
            }
            droppedPowerUpObject.transform.position += -Vector3.up * 0.01f;
            if (droppedPowerUpObject.transform.position.y <= -3)
            {
                DestroyPowerUp(droppedPowerUpObject);
            }
        }
    }

    public void DropRandomPowerUp(Vector3 dropPosition)
    {
        int selectedPowerUpIndex = UnityEngine.Random.Range(0, powerUpKeysList.Count - 1);

        String powerUpKey = powerUpKeysList[selectedPowerUpIndex];

        GameObject powerUpObject = Instantiate(powerUpPrefabReference);

        powerUpObject.transform.position = new Vector3(dropPosition.x, dropPosition.y, dropPosition.z - 1.0f);

        activeDroppedPowerUps.Add(powerUpObject);

        switch (powerUpKey)
        {
            case "MultipleBalls":
                break;
            case "ExtraLive":
                break;
            case "FasterBall":
                break;
        }

    }

    private void ApplyPowerUp()
    {

    }

    private void DestroyPowerUp(GameObject powerUpToDestroy)
    {
        if (!activeDroppedPowerUps.Contains(powerUpToDestroy))
        {
            return;
        }
        activeDroppedPowerUps.Remove(powerUpToDestroy);
        Destroy(powerUpToDestroy);
    }

    private void OnBlockDestruction(int blockPoints, Vector3 blockPosition)
    {
        DropRandomPowerUp(blockPosition);
    }

    public void MultipleBalls()
    {

    }

}

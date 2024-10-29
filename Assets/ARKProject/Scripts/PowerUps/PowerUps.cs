using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PowerUps : MonoBehaviour
{

     public enum PowerUpType
    {
        ExtraLive
    }

    public GameObject powerUpPrefabReference;
    

    void OnEnable() 
    {
        BlockBehaviour.OnBlockDestructionEvent += OnBlockDestruction;
        PowerUp.PowerUpDestroyed += OnPowerUpDestroyed;
    }

    void OnDisable() 
    {
        BlockBehaviour.OnBlockDestructionEvent -= OnBlockDestruction;
        PowerUp.PowerUpDestroyed -= OnPowerUpDestroyed;
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void DropRandomPowerUp(Vector3 dropPosition)
    {
        PowerUpType powerUpKey = GetRandomPowerUp();

        GameObject powerUpObject = Instantiate(powerUpPrefabReference);
        PowerUp powerUpClass = powerUpObject.GetComponent<PowerUp>();
        powerUpClass.SetPowerUpType(powerUpKey);

        powerUpObject.transform.position = new Vector3(dropPosition.x, dropPosition.y, dropPosition.z - 1.3f);
    }

    public static PowerUpType GetRandomPowerUp()
    {

        Array values = Enum.GetValues(typeof(PowerUpType));
        PowerUpType randomPowerUp = (PowerUpType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        
        return randomPowerUp;
    }

    private bool CanDropPowerUp()
    {
        int randomValue = UnityEngine.Random.Range(0, 10);
        if (randomValue >= 4)
        {
            return false;
        }
        return true;
    }
    private void OnBlockDestruction(int blockPoints, Vector3 blockPosition)
    {
        if (!CanDropPowerUp())
        {
            return;
        }
        DropRandomPowerUp(blockPosition);
    }
    
    private void OnPowerUpDestroyed(PowerUpType powerUpType)
    {

    }
};

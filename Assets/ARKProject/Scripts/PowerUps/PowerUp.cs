using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public delegate void OnPowerUpDestroyed(PowerUps.PowerUpType powerUpType);
    public static event OnPowerUpDestroyed PowerUpDestroyed;

    private GameObject powerUpObject;
    private PowerUps.PowerUpType currentPowerUpType;

    
    void Start()
    {
        powerUpObject = this.gameObject;
    }

   
    void Update()
    {
        powerUpObject.transform.position += Vector3.down * Time.deltaTime * 10.0f;
        if (powerUpObject.transform.position.y <= -3)
        {
            DestroyPowerUp();
        }
    }

    public void SetPowerUpType(PowerUps.PowerUpType powerUpType)
    {
        currentPowerUpType = powerUpType;
    }

    private void ApplyPowerUp()
    {
        switch (currentPowerUpType)
        {
            case PowerUps.PowerUpType.ExtraLive:
                ExtraLive();
                break;
        }

        DestroyPowerUp();
    }

    private void DestroyPowerUp()
    {
        PowerUpDestroyed(currentPowerUpType);
        Destroy(powerUpObject);
    }


    private void OnCollisionEnter(Collision other) 
    {
        if (other.transform.tag == "DeathVolume")
        {
            DestroyPowerUp();
        }    
        else if(other.transform.tag == "Paddle")
        {
            ApplyPowerUp();
        }
    }

    public void ExtraLive()
    {
        if (ARKGameMode.Instance == null)
        {
            return;
        }
        ARKGameMode.Instance.AddLives(1);
    }

    public void FasterBall()
    {

    }

}

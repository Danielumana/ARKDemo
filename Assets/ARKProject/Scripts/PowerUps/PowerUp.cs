using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public delegate void OnPowerUpDestroyed(PowerUps.PowerUpType powerUpType, bool bApllied);
    public static event OnPowerUpDestroyed PowerUpDestroyed;

    private GameObject powerUpObject;
    private PowerUps.PowerUpType currentPowerUpType;

    
    void Start()
    {
        powerUpObject = this.gameObject;
        powerUpObject.GetComponent<MeshRenderer>().material.color = Color.cyan;
    }

   
    void Update()
    {
        powerUpObject.transform.position += Vector3.down * Time.deltaTime * 7.0f;
        if (powerUpObject.transform.position.y <= -3)
        {
            DestroyPowerUp(false);
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
        DestroyPowerUp(true);
    }

    private void DestroyPowerUp(bool bApplied)
    {
        PowerUpDestroyed(currentPowerUpType, bApplied);
        Destroy(powerUpObject);
    }


    private void OnCollisionEnter(Collision other) 
    {
        if (other.transform.tag == "DeathVolume")
        {
            DestroyPowerUp(false);
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

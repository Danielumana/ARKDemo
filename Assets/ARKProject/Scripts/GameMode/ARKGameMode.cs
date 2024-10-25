using System;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ARKGameMode : MonoBehaviour
{

    public enum GameState
    {
        MainMenu,
        LevelTransition,
        Playing,
        Paused,
        GameOver
    }

    // Declara un delegado para el cambio de estado
    public delegate void OnGameStateChanged(GameState newState);
    public static event OnGameStateChanged GameStateChanged;

    // Estado actual del juego
    private GameState currentState;
    public GameObject paddleReference;
    public GameObject ballPrefab;
    public GameObject ballsPoolReference;
    private List<GameObject> activeBalls = new List<GameObject> {};
    public GameObject deathVolume;
    public Vector3 ballsInPoolPosition = new Vector3(0,1000,0);
    public int playerLives = 3;
    private int playerScore = 0;

    public static ARKGameMode Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
      ResetToInitialBall();
    }

    
    void Update()
    {
        
    }

    public void AddPointsToScore(int pointsToAdd)
    {
        playerScore += pointsToAdd;
    }

    public void RemoveLives(int livesToRemove)
    {
        playerLives -= livesToRemove;
        if (livesToRemove <= 0)
        {
            OnGameOver();
        }
        else
        {
            
        }
    }

    public GameObject SpawnBall(Vector3 ballSpawnPosition)
    {
        GameObject spawnedBall = GetBallFromPool("Idle");
        if (spawnedBall == null)
        {
            if (ballPrefab == null)
            {
                return null;    
            }
            
            spawnedBall = Instantiate(ballPrefab);
        }
        if (spawnedBall == null)
        {
            return null;
        }
        spawnedBall.transform.position = ballSpawnPosition;
        SetBallBehaviourEnable(spawnedBall, true);
        int spawnedBallInListIndex = activeBalls.FindIndex(obj => obj == spawnedBall);
        if (spawnedBallInListIndex >= 0)
        {
            return spawnedBall;
        }
        activeBalls.Add(spawnedBall);
        MoveBallToPool(spawnedBall, "Active");
        
        return spawnedBall;
    }
    public void DespawnBall(GameObject ballToRemove)
    {
        if (ballToRemove == null)
        {
            return;
        }
        int objectIndexToRemove = activeBalls.FindIndex(obj => obj == ballToRemove);
        if (objectIndexToRemove < 0)
        {
            return;
        }
        activeBalls.RemoveAt(objectIndexToRemove);
        MoveBallToPool(ballToRemove, "Idle");

        if (activeBalls.Count <= 0)
        {
            RemoveLives(1);
            ResetToInitialBall();
        }
        
        
    }

    public void DespawnAllActiveBalls()
    {
        foreach (GameObject activeBall in activeBalls)
        {
            if (activeBall == null)
            {
                activeBalls.Remove(activeBall);
                continue;
            }
            DespawnBall(activeBall);
        }
    }

    private void ResetToInitialBall()
    {
        if (paddleReference == null)
        {
            return;
        }
        DespawnAllActiveBalls();
        Transform initialBallPositionTransform = paddleReference.transform.Find("IntialBallPosition");
        if (initialBallPositionTransform == null)
        {
            return;
        }
        SpawnBall(initialBallPositionTransform.position);
    }

    private GameObject GetBallFromPool(String poolName)
    {
        if (poolName == null)
        {
            return null;
        }
        if (ballsPoolReference == null)
        {
            return null;
        }
        
        Transform requredBallsPoolTransform = ballsPoolReference.transform.Find(poolName);
        if (requredBallsPoolTransform == null)
        {
            return null;
        }

        int ballsInPoolCount = requredBallsPoolTransform.childCount;
        if (ballsInPoolCount <= 0)
        {
            return null;
        }

        Transform ballInPoolTransform = requredBallsPoolTransform.GetChild(0);
        if (ballInPoolTransform == null)
        {
            return null;
        }

        return ballInPoolTransform.gameObject;
    }

    private void MoveBallToPool(GameObject ballReference, String poolName)
    {
        if (ballReference == null)
        {
            return;
        }
        if (ballReference.transform.parent.name == poolName)
        {
            return;
        }
        Transform requiredBallsPoolTransform = ballsPoolReference.transform.Find(poolName);
        if (requiredBallsPoolTransform == null)
        {
            return;
        }
        GameObject activeBallsPoolReference = requiredBallsPoolTransform.gameObject;
        if (activeBallsPoolReference == null)
        {
            return;
        }
        ballReference.transform.parent = activeBallsPoolReference.transform;
        if (poolName == "Idle")
        {
            ballReference.transform.position = ballsInPoolPosition; 
        }
    }

    private void SetBallBehaviourEnable(GameObject ballReference, bool bEnable)
    {
        if (ballReference == null)
        {
            return;
        }
        BallMovement ballMovementClass = ballReference.GetComponent<BallMovement>();
        if (ballMovementClass == null)
        {
            return;
        }
        ballMovementClass.enabled = bEnable;
    }

    private void OnGameOver()
    {
        
    }

}

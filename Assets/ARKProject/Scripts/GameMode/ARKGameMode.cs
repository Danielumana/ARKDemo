using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ARKGameMode : MonoBehaviour
{

    public enum GameState
    {
        InitialBall,
        Playing,
        Paused,
        GameWin,
        GameLost
    }

    public delegate void OnGameStateChanged(GameState newState, GameState oldState);
    public static event OnGameStateChanged GameStateChanged;

    private GameState currentGameState;
    private GameState lastGameStateBeforePause;
    public GameObject paddleReference;
    public GameObject levelBlocksContainer;
    public GameObject ballPrefab;
    public GameObject ballsPoolReference;
    private GameObject mainBallReference;
    public GameObject deathVolume;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    private List<GameObject> activeBalls = new List<GameObject> {};
    public Vector3 ballsInPoolPosition = new Vector3(0,1000,0);
    private int levelBlocksCount;
    private int playerScore = 0;
    private int internalPlayerLives;

    public static ARKGameMode Instance { get; private set; }

    void OnEnable() 
    {
        PlayerMovement.InitialImpulseAction += OnInitialImpulseAction;
        PlayerMovement.PauseAction += OnPauseAction;
        BlockBehaviour.OnBlockDestructionEvent += OnBlockDestruction;
    }

    void OnDisable() 
    {
        PlayerMovement.InitialImpulseAction -= OnInitialImpulseAction;
        PlayerMovement.PauseAction -= OnPauseAction;
        BlockBehaviour.OnBlockDestructionEvent -= OnBlockDestruction;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        internalPlayerLives = PlayerData.Instance.GetPlayerLives();
        livesText.SetText(internalPlayerLives.ToString());
        playerScore = PlayerData.Instance.GetPlayerScore();
        scoreText.SetText(new String("Points: "+playerScore.ToString()));
        ResetToInitialBall();
        print("Started::: Score= "+playerScore+" Lives= "+internalPlayerLives);
        if (levelBlocksContainer == null)
        {
            return;
        }
        levelBlocksCount = levelBlocksContainer.transform.childCount; 
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
        internalPlayerLives -= livesToRemove;
        livesText.SetText(internalPlayerLives.ToString());
        print("Lives= "+internalPlayerLives);
        print("Score= "+playerScore);
        if (internalPlayerLives <= 0)
        {
            OnGameOver(GameState.GameLost);
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

        if (activeBalls.Count <= 0 && currentGameState != GameState.GameWin)
        {
            RemoveLives(1);
            if (currentGameState != GameState.GameLost)
            {
                ResetToInitialBall();   
            }
        }
    }

    public void DespawnAllActiveBalls()
    {
        List<GameObject> activeBallsCopy = new List<GameObject>(activeBalls);
        foreach (GameObject activeBall in activeBallsCopy)
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
        SetCurrentGameState(GameState.InitialBall);
        GameObject spawnedBall = SpawnBall(initialBallPositionTransform.position);
         if (activeBalls.Count == 1 )
        {
            mainBallReference = spawnedBall;
        }
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

    private void OnGameOver(GameState gameOverState )
    {
        SetCurrentGameState(gameOverState);
        DespawnAllActiveBalls();
        print("GameOver ="+currentGameState);
    }

    public GameObject GetMainBallReference()
    {
        return mainBallReference;
    }

    public int GetInternalPlayerLives()
    {
        return internalPlayerLives;
    }
    public int GetInternalPlayerScore()
    {
        return playerScore;
    }

    private void SetCurrentGameState(GameState newState)
    {
        GameState oldGameState = currentGameState;
        currentGameState = newState;
        GameStateChanged(currentGameState, oldGameState);
    }
    public GameState GetCurrentGameState()
    {
        return currentGameState;
    }

    private void OnInitialImpulseAction(Vector3 impulseDirection)
    {
        if (currentGameState != GameState.InitialBall)
        {
            return;
        }
        SetCurrentGameState(GameState.Playing);
    }

    private void OnBlockDestruction(int destructionPoints)
    {
        AddPointsToScore(destructionPoints);
        levelBlocksCount -= 1;
        scoreText.SetText(new String("Points: "+playerScore.ToString()));
        if (levelBlocksCount <= 0)
        {
            OnGameOver(GameState.GameWin);
        }
    }

    private void OnPauseAction()
    {
        if (currentGameState == GameState.GameLost || currentGameState == GameState.GameWin)
        {
            return;
        }
        if (currentGameState == GameState.Paused)
        {
            SetCurrentGameState(lastGameStateBeforePause);
        }
        else
        {
            lastGameStateBeforePause = currentGameState;
            SetCurrentGameState(GameState.Paused);
        }
        Time.timeScale = currentGameState == GameState.Paused ? 0 : 1; 
    }
    
    public void ForcePause()
    {
        OnPauseAction();
    }
}

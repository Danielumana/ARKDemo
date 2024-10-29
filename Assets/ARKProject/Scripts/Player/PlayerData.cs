using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public int intialPlayerLives;
    private int playerLives;
    private int playerScore;
    private int bestScore;

    public static PlayerData Instance { get; private set; }
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
    void OnEnable() 
    {
        ARKGameMode.GameStateChanged += OnGameStateChanged; 
    }

    void OnDisable() 
    {
        ARKGameMode.GameStateChanged -= OnGameStateChanged;
    }
    void Start()
    {
        playerLives = intialPlayerLives;
        playerScore = 0;
    }

    void Update()
    {
        
    }

    public int GetPlayerLives()
    {
        return playerLives;
    }
    
    public int GetPlayerScore()
    {
        return playerScore;
    }

    private void OnGameStateChanged(ARKGameMode.GameState newState, ARKGameMode.GameState oldState)
    {
        switch (newState)
        {
            case ARKGameMode.GameState.GameWin:
                
                int inPlayerLives = ARKGameMode.Instance.GetInternalPlayerLives();
                int inPlayerScore = ARKGameMode.Instance.GetInternalPlayerScore();

                playerLives = inPlayerLives;
                playerScore = inPlayerScore;
                break;
            case ARKGameMode.GameState.GameLost:
                playerLives = intialPlayerLives;
                playerScore = 0;
                break;
            
        }
    }
}

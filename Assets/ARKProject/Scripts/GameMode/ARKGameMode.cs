using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ARKGameMode : MonoBehaviour
{
    public GameObject paddleReference;
    public GameObject ballReference;
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
       
    }

    
    void Update()
    {
        
    }

    public void AddPointsToScore(int pointsToAdd)
    {
        playerScore += pointsToAdd;
        Debug.Log("Score: "+playerScore);
    }
}

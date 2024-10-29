using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelTravelingManager : MonoBehaviour
{
    public float waitingSecondsToTravel;
    private int levelsCount;
    private int currentLevelIndex;

    public static LevelTravelingManager Instance { get; private set; }
    
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
        levelsCount = SceneManager.sceneCountInBuildSettings;
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
    }

    void Update()
    {

    }

    public void LoadRequiredLevelWithDelay(int requiredLevelIndex)
    {
        if (currentLevelIndex < 0 || requiredLevelIndex < 0 || requiredLevelIndex > levelsCount - 1)
        {
            return;
        }
        StartCoroutine(WaitAndLoadRequiredLevel(requiredLevelIndex));
    }

    private IEnumerator WaitAndLoadRequiredLevel(int requiredLevelIndex)
    {
        yield return new WaitForSeconds(waitingSecondsToTravel);
        SceneManager.LoadScene(requiredLevelIndex);
        currentLevelIndex = requiredLevelIndex;
    }

    public void LoadNextLevel()
    {
        int nextLevelIndex = currentLevelIndex + 1 > levelsCount - 1 ? 1 : currentLevelIndex + 1;
        LoadRequiredLevelWithDelay(nextLevelIndex);
    }

    public void ReloadCurrentLevel()
    {
        if (ARKGameMode.Instance == null)
        {
            return;
        }
        LoadRequiredLevelWithDelay(currentLevelIndex);
    }

     private void OnGameStateChanged(ARKGameMode.GameState newState, ARKGameMode.GameState oldState)
    {
        
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelTravelingManager : MonoBehaviour
{
    public float waitingSecondsToTravel;

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
        
    }

    void Update()
    {
        
    }

    public void LoadNextLevelWithDelay()
    {
        StartCoroutine(WaitAndLoadNextScene());
    }

    private IEnumerator WaitAndLoadNextScene()
    {
        yield return new WaitForSeconds(waitingSecondsToTravel);
        SceneManager.LoadScene("Level_2");
    }

    private void OnGameStateChanged(ARKGameMode.GameState newState, ARKGameMode.GameState oldState)
    {
        if (newState == ARKGameMode.GameState.GameWin)
        {
            LoadNextLevelWithDelay();
        }
    }
}

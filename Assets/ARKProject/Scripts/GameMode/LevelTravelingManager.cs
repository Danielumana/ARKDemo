using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelTravelingManager : MonoBehaviour
{
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
        yield return new WaitForSeconds(3);
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

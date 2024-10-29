using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuActions : MonoBehaviour
{
    private GameObject currentMenuObject;
    private GameObject persistentObject;
    void OnEnable() 
    {
        PlayerMovement.PauseAction += OnPauseAction;
        ARKGameMode.GameStateChanged += OnGameStateChanged; 
    }
    
    void OnDisable() 
    {
        PlayerMovement.PauseAction += OnPauseAction;
        ARKGameMode.GameStateChanged += OnGameStateChanged;
    }

    void Start()
    {
        persistentObject = PlayerData.Instance.gameObject;

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            SetActiveMenuByName("MainMenu");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLevelSelected(int levelIndex)
    {
        SetCurrentMenuEnable(false);
        LevelTravelingManager.Instance.LoadRequiredLevelWithDelay(levelIndex);
    }

    public void ResetLevel()
    {
        if (LevelTravelingManager.Instance == null || ARKGameMode.Instance == null)
        {
            return;
        }
        SetCurrentMenuEnable(false);
        if (ARKGameMode.Instance.GetCurrentGameState() == ARKGameMode.GameState.Paused)
        {
            ARKGameMode.Instance.ForcePause();
        }
        LevelTravelingManager.Instance.ReloadCurrentLevel();
        Transform nextLevelButtonTransform = gameObject.transform.Find("PauseMenu/Panel/NextLevelButton");
        if (nextLevelButtonTransform == null)
        {
            return;
        }
        GameObject nextLevelButton = nextLevelButtonTransform.gameObject;
        if (nextLevelButton == null)
        {
            return;
        }
        nextLevelButton.SetActive(false);

        Transform gameOverTextTransform = gameObject.transform.Find("PauseMenu/Panel/GameOverText");
        if (gameOverTextTransform == null)
        {
            return;
        }
        TextMeshProUGUI gameOverText = gameOverTextTransform.GetComponent<TextMeshProUGUI>();
        if (gameOverText == null)
        {
            return;
        }
        gameOverText.enabled = false;
    }

    public void MainMenu()
    {
        if (LevelTravelingManager.Instance == null)
        {
            return;
        }
        if (ARKGameMode.Instance.GetCurrentGameState() == ARKGameMode.GameState.Paused)
        {
            ARKGameMode.Instance.ForcePause();
        }
        LevelTravelingManager.Instance.LoadRequiredLevelWithDelay(0);
        SetActiveMenuByName("MainMenu");
    }

    public void NextLevel()
    {
        if (LevelTravelingManager.Instance == null)
        {
            return;
        }
        SetCurrentMenuEnable(false);
        LevelTravelingManager.Instance.LoadNextLevel();

        Transform nextLevelButtonTransform = gameObject.transform.Find("PauseMenu/Panel/NextLevelButton");
        if (nextLevelButtonTransform == null)
        {
            return;
        }
        GameObject nextLevelButton = nextLevelButtonTransform.gameObject;
        if (nextLevelButton == null)
        {
            return;
        }
        nextLevelButton.SetActive(false);

        Transform gameOverTextTransform = gameObject.transform.Find("PauseMenu/Panel/GameOverText");
        if (gameOverTextTransform == null)
        {
            return;
        }
        TextMeshProUGUI gameOverText = gameOverTextTransform.GetComponent<TextMeshProUGUI>();
        if (gameOverText == null)
        {
            return;
        }
        gameOverText.enabled = false;
    }

    private void OnPauseAction()
    {
        if (ARKGameMode.Instance == null)
        {
            return;
        }
        SetActiveMenuByName("PauseMenu");
    }

    private void SetCurrentMenuEnable(bool bEnable)
    {
        if (currentMenuObject == null)
        {
            return;
        }
        currentMenuObject.SetActive(bEnable);
        if (bEnable == false)
        {
            currentMenuObject = null;
        }
    }

    private void SetActiveMenuByName(String menuToActivate)
    {
        GameObject oldMenuObject = currentMenuObject;
        SetCurrentMenuEnable(false);
        if (oldMenuObject != null && oldMenuObject.name == menuToActivate)
        {
            return;
        }
        if (persistentObject == null)
        {
            return;
        }
        Transform menuObjectTransform = persistentObject.transform.Find(menuToActivate);
        if (menuObjectTransform == null)
        {
            return;
        }
        GameObject menuObjectReference = menuObjectTransform.gameObject;
        if (menuObjectReference == null)
        {
            return;
        }
        currentMenuObject = menuObjectReference;
        SetCurrentMenuEnable(true);
    }

    private void OnGameStateChanged(ARKGameMode.GameState newState, ARKGameMode.GameState oldState)
    {
        if (newState == ARKGameMode.GameState.GameWin || newState == ARKGameMode.GameState.GameLost)
        {
            Transform gameOverTextTransform = gameObject.transform.Find("PauseMenu/Panel/GameOverText");
            if (gameOverTextTransform == null)
            {
                return;
            }
            TextMeshProUGUI gameOverText = gameOverTextTransform.GetComponent<TextMeshProUGUI>();
            if (gameOverText == null)
            {
                return;
            }
            
            String newText = newState == ARKGameMode.GameState.GameWin ? "GAME WIN" : "GAME LOST";
            gameOverText.SetText(newText);
            gameOverText.enabled = true;
            
            OnPauseAction();
            if (newState == ARKGameMode.GameState.GameWin)
            {
                Transform nextLevelButtonTransform = gameObject.transform.Find("PauseMenu/Panel/NextLevelButton");
                if (nextLevelButtonTransform == null)
                {
                    return;
                }
                GameObject nextLevelButton = nextLevelButtonTransform.gameObject;
                if (nextLevelButton == null)
                {
                    return;
                }
                nextLevelButton.SetActive(true);
            }
        }
    }

}

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuActions : MonoBehaviour
{
    private GameObject currentMenuObject;
    private GameObject persistentObject;
    void OnEnable() 
    {
        PlayerMovement.PauseAction += OnPauseAction;
    }
    
    void OnDisable() 
    {
        PlayerMovement.PauseAction -= OnPauseAction;
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

    private void OnPauseAction()
    {
        print("asdasd");
        if (ARKGameMode.Instance == null)
        {
            return;
        }
         print("asdasd");
        ARKGameMode.GameState currentGameSate = ARKGameMode.Instance.GetCurrentGameState();
        if (currentGameSate == ARKGameMode.GameState.GameLost ||currentGameSate ==  ARKGameMode.GameState.GameWin)
        {
            return;
        }
         print("asdasd");
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

    private GameObject GetMainCamera()
    {
        
        return null;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class PauseFSM : MonoBehaviour
{
    public enum GameState
    {
        Play,
        Pause
    }

    public GameObject pauseMenuUI;
    private GameState currentState;
    private PlayerMovement playerController;

    private bool isSettingsOpen = false;

    void Start()
    {
        currentState = GameState.Play;
        Time.timeScale = 1f;
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
        playerController = FindObjectOfType<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Play || (currentState == GameState.Pause && !isSettingsOpen))
                TogglePause().Forget();
            else 
                return;
        }
    }
    
    private async UniTaskVoid TogglePause()
    {
        if(currentState == GameState.Play) currentState = GameState.Pause;
        else if(currentState == GameState.Pause) currentState = GameState.Play;
        bool isPaused = currentState == GameState.Pause;
        Time.timeScale = isPaused ? 0 : 1;
        if (isPaused)
        {
            if (playerController != null)
                playerController.ToggleControls(false);
            await ShowPauseMenu();
        }
        else
        {
            if (playerController != null)
                playerController.ToggleControls(true);
            await HidePauseMenu();
        }
    }

    private async UniTask ShowPauseMenu()
    {
        pauseMenuUI.SetActive(true);
        await UniTask.Delay(0); 
    }

    private async UniTask HidePauseMenu()
    {
        pauseMenuUI.SetActive(false);
        await UniTask.Delay(0); 
    }

    public GameState CurrentState
    {
        get { return currentState; }
    }

    public void setSettingsOpen(bool isOpen){
        isSettingsOpen = isOpen;
    }
}

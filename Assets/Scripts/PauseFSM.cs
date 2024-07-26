using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if (currentState == GameState.Play)
                PauseGame();
            else if (currentState == GameState.Pause && !isSettingsOpen)
                ResumeGame();
            else 
                return;
        }
    }

    void PauseGame()
    {
        currentState = GameState.Pause;
        Time.timeScale = 0f; 
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);
        if (playerController != null)
            playerController.ToggleControls(false);
    }

    void ResumeGame()
    {
        currentState = GameState.Play;
        Time.timeScale = 1f; 
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
        if (playerController != null)
            playerController.ToggleControls(true);
    }

    public GameState CurrentState
    {
        get { return currentState; }
    }

    public void setSettingsOpen(bool isOpen){
        isSettingsOpen = isOpen;
    }
}

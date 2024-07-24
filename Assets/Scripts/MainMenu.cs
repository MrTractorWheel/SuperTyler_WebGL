using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button[] buttons;
    [SerializeField] GameObject levels;
    
    private void DisplayStars()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int stars = PlayerPrefs.GetInt("Level" + i + "Stars", 0);
            bool allLives = PlayerPrefs.GetInt("Level" + i + "AllLives", 0) == 1;
            // Put as many stars as earned
            // Change color of stars to red if allLives is true
        }
    }

    private void Awake(){
        ButtonsToArray();
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        for (int i = 0; i < unlockedLevel; i++)
        {
            buttons[i].interactable = true;
        }
        DisplayStars();
    }

    public void OpenLevel(int levelId){
        SceneManager.LoadSceneAsync(levelId);
    }

    public void QuitGame(){
        Application.Quit();
    }

    private void ButtonsToArray(){
        int buttonCount = levels.transform.childCount;
        buttons = new Button[buttonCount];
        for(int i=0; i<buttonCount; i++){
            buttons[i] = levels.transform.GetChild(i).gameObject.GetComponent<Button>();
        }
    }
}
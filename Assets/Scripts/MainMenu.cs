using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button[] buttons;
    [SerializeField] GameObject levels;

    [SerializeField] Sprite filledStarSprite;
    [SerializeField] Sprite emptyStarSprite;
    [SerializeField] Sprite redStarSprite;
    
   private void DisplayStars(){
        if (buttons == null || buttons.Length == 0){
            Debug.LogWarning("No buttons assigned to the buttons array.");
            return;
        }
        for (int i = 0; i < buttons.Length; i++){
            Transform starContainer = buttons[i].transform.Find("Stars");
            if (starContainer == null){
                Debug.LogWarning("Star container not found for button " + i);
                continue;
            }
            int stars = PlayerPrefs.GetInt("Level" + (i+1) + "Stars", 0);
            bool allLives = PlayerPrefs.GetInt("Level" + (i+1) + "AllLives", 0) == 1;
            for (int j = 0; j < 3; j++){
                if (j >= starContainer.childCount){
                    Debug.LogWarning("Not enough star images in container for button " + i);
                    continue;
                }
                Image starImage = starContainer.GetChild(j).GetComponent<Image>();
                if (starImage == null){
                    Debug.LogWarning("Star image component missing in button " + i + ", star index " + j);
                    continue;
                }
                if (stars == 3){
                    starImage.sprite = allLives ? redStarSprite : filledStarSprite;
                }
                else if (stars == 2){
                    starImage.sprite = (j == 2) ? emptyStarSprite : filledStarSprite;
                }
                else if (stars == 1){
                    starImage.sprite = (j == 0) ? filledStarSprite : emptyStarSprite;
                }
                else{
                    starImage.sprite = emptyStarSprite;
                }
            }
        }
    }

    private void Awake(){
        ButtonsToArray();
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        if (unlockedLevel >= 5) return;
        for (int i = 0; i < buttons.Length; i++){
            buttons[i].interactable = false;
        }
        for (int i = 0; i < unlockedLevel; i++){
            buttons[i].interactable = true;
        }
    }

    private void Start(){
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
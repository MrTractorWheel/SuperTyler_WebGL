using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;
    public int axeCount;
    public int machetteCount;
    [SerializeField] TextMeshProUGUI livesTxt;
    [SerializeField] TextMeshProUGUI scoreTxt;
    [SerializeField] TextMeshProUGUI axeCountTxt;
    [SerializeField] TextMeshProUGUI machetteCountTxt;
    [SerializeField] Image axeImage;
    [SerializeField] Image machetteImage;
    [SerializeField] Color originalColor;
    [SerializeField] Color highlightedColor;
    private int starsEarned = 0;
    private bool allLivesRemaining = false;
    [SerializeField] int starLimit1;
    [SerializeField] int starLimit2;
    [SerializeField] int starLimit3;

    public void CalculateStars()
    {
        if (score >= starLimit3) 
            starsEarned = 3;
        else if (score >= starLimit2)
            starsEarned = 2;
        else if (score >= starLimit1)
            starsEarned = 1;
        else
            starsEarned = 0;

        allLivesRemaining = playerLives == 3;
    }

    public void SaveStarProgress(int levelIndex)
    {
        int previousStars = PlayerPrefs.GetInt("Level" + levelIndex + "Stars", 0);
        if (starsEarned > previousStars || (starsEarned == previousStars && allLivesRemaining))
        {
            PlayerPrefs.SetInt("Level" + levelIndex + "Stars", starsEarned);
            PlayerPrefs.SetInt("Level" + levelIndex + "AllLives", allLivesRemaining ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    void Awake()
    {
        int GameSessionCount = FindObjectsOfType<GameSession>().Length;
        if (GameSessionCount > 1){
            Destroy(gameObject);
        }
        else{
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start(){
        ChangeColor(true);
        livesTxt.text = playerLives.ToString();
        scoreTxt.text = score.ToString();
        axeCountTxt.text = axeCount.ToString();
        machetteCountTxt.text = machetteCount.ToString();
    }

    public void ProcessPlayerDeath(){
        if(playerLives > 1){
            TakeLife().Forget();
        }
        else{
            ResetGameSession().Forget();
        }
    }

    public void ResetGame(){
        ResetGameSession().Forget();
    }

    public void ToScore(int points){
        score += points;
        scoreTxt.text = score.ToString();
    }

    public void TakeAxe(int num){
        axeCount+=num;
        axeCountTxt.text = axeCount.ToString();
    }

    public void TakeMachette(int num){
        machetteCount+=num;
        machetteCountTxt.text = machetteCount.ToString();
    }

    public void throwAxe(){
        axeCount--;
        axeCountTxt.text = axeCount.ToString();
    }

    public void throwMachette(){
        machetteCount--;
        machetteCountTxt.text = machetteCount.ToString();
    }

    private async UniTaskVoid TakeLife()
    {
        playerLives--;
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        await SceneManager.LoadSceneAsync(currentSceneIndex);
        livesTxt.text = playerLives.ToString();
    }

    private async UniTaskVoid ResetGameSession()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        await SceneManager.LoadSceneAsync(0);
        Destroy(gameObject);
    }

     public void ChangeColor(bool isAxe)
    {
        if (isAxe){
            axeImage.color = highlightedColor;
            machetteImage.color = originalColor;
            axeCountTxt.color = highlightedColor;
            machetteCountTxt.color = originalColor;
            axeCountTxt.text = axeCount.ToString();
            machetteCountTxt.text = machetteCount.ToString();
        }
        else{
            axeImage.color = originalColor;
            machetteImage.color = highlightedColor;
            axeCountTxt.color = originalColor;  
            machetteCountTxt.color = highlightedColor;
            axeCountTxt.text = axeCount.ToString();
            machetteCountTxt.text = machetteCount.ToString();
        }
    }

}

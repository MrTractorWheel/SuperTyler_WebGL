using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;
    [SerializeField] int axeCount;
    [SerializeField] int machetteCount;
    [SerializeField] TextMeshProUGUI livesTxt;
    [SerializeField] TextMeshProUGUI scoreTxt;
    [SerializeField] TextMeshProUGUI axeCountTxt;
    [SerializeField] TextMeshProUGUI machetteCountTxt;
    [SerializeField] Image axeImage;
    [SerializeField] Image machetteImage;
    [SerializeField] Color originalColor;
    [SerializeField] Color highlightedColor;
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
            TakeLife();
        }
        else{
            ResetGameSession();
        }
    }

    public void ResetGame(){
        ResetGameSession();
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

    private void TakeLife()
    {
        playerLives--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesTxt.text = playerLives.ToString();
    }

    private void ResetGameSession()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);
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

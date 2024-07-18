using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButtonControl : MonoBehaviour
{
    public void LoadMainMenu(){
        FindObjectOfType<GameSession>().ResetGame();
    }

    public void LoadControls(){
        //TO DO: add controls UI & complete
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    public void StartGame()
    {
        // Load the game scene (assuming the game scene is named "Game")
        SceneManager.LoadScene("Game");
    }
}

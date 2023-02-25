using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeMenu : MonoBehaviour
{
    private GameManager gameManager;

    void Awake()
    {
        if (gameManager == null)
        {
            gameManager = GetComponentInParent<Player>().gameManager;
        }

    }

    public void SaveGame()
    {
        gameManager.SaveGame();
    }


    public void ReturnToMenu()
    {
        // resume time
        Time.timeScale = 1.0f;
        AudioListener.pause = false;

        gameManager.ReturnToMenu();
    }
}

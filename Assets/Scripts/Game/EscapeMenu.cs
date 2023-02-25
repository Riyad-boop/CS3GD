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
            gameManager = GetComponentInParent<GameManager>();
        }

    }

    public void SaveGame()
    {
        gameManager.SaveGame();
    }


    public void QuitGame()
    {
        Debug.Log("Quit");
        //TODO return to menu
        //Application.Quit();
    }
}

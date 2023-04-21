using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeMenu : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private GameObject optionsMenu;

    void Awake()
    {
        LoadEscsapeMenu();
        if (gameManager == null)
        {
            gameManager = GetComponentInParent<Player>().gameManager;
        }

    }

    public void SaveGame()
    {
        gameManager.SaveGame();
    }

    public void LoadOptionsMenu()
    {
        menu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void LoadEscsapeMenu()
    {
        optionsMenu.SetActive(false);
        menu.SetActive(true);
    }


    public void ReturnToMenu()
    {
        // resume time
        Time.timeScale = 1.0f;
        AudioListener.pause = false;

        gameManager.ReturnToMenu();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    // Start is called before the first frame update
    public void NewGame()
    {
        gameManager.NewGame();
    }

    public void ConintueGame()
    {
        gameManager.LoadGame();
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}

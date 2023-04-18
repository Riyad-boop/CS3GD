using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject LevelCompleteMenu;
    public int score;

    private ZombieSpawner enemySpawner;
    private PlayerSpawner playerSpawner;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        //This is only for test level
        if(SceneManager.GetActiveScene().name == "Test_Level")
        {
            playerSpawner = GameObject.FindGameObjectWithTag("PlayerSpawner").GetComponent<PlayerSpawner>();
            playerSpawner.SpawnNewPlayer(3,0,this);
            enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<ZombieSpawner>();
            enemySpawner.NewGameSpawn(this);
        }
      
    }

    public void NewGame()
    {
        StartCoroutine(LoadGameScene(1,true));

    }

    public void SaveGame()
    {
        enemySpawner.SaveZombies();
        playerSpawner.SavePlayer();
        Debug.Log("Game Saved");
    }

    /// <summary>
    /// loads the scene depending on the level variable in the player's save data
    /// </summary>
    public void LoadGame()
    {
        PlayerData data = SaveLoadSystem.LoadPlayer();
        if (data != null )
        {
            StartCoroutine(LoadGameScene(data.level,false));
        }
        else
        {
            Debug.Log("No save file found");
        }

    }

    /// <summary>
    /// Asynchronous function handles loading the game scene, then calls the ingame spawners to either spawn new entities or load using saved entity data
    /// </summary>
    /// <param name="level"></param>
    /// <param name="newGame"></param>
    /// <returns></returns>
    private IEnumerator LoadGameScene(int level, bool newGame)
    {
        AsyncOperation sceneLoader;

        //sceneLoader = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + level);
        sceneLoader = SceneManager.LoadSceneAsync(level);

        while (!sceneLoader.isDone)
        {
            yield return null;
        }

        //once the scene has loaded then start new game or load game
        // player needs to spawned first to send its transform to other dependant gameobjects
        if (newGame)
        {
            playerSpawner = GameObject.FindGameObjectWithTag("PlayerSpawner").GetComponent<PlayerSpawner>();
            playerSpawner.SpawnNewPlayer(level,score,this);
            enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<ZombieSpawner>();
            enemySpawner.NewGameSpawn(this);
        }
        else
        {
            playerSpawner = GameObject.FindGameObjectWithTag("PlayerSpawner").GetComponent<PlayerSpawner>();
            playerSpawner.LoadPlayer(this);
            enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<ZombieSpawner>();
            enemySpawner.LoadZombies(this);
        }
    }
    
    private IEnumerator LoadScene(int buildIndex)
    {
        AsyncOperation sceneLoader;
        sceneLoader = SceneManager.LoadSceneAsync(buildIndex);

        while (!sceneLoader.isDone)
        {
            yield return null;
        }

        // resume time
        Time.timeScale = 1.0f;
        AudioListener.pause = false;
    }

    /// <summary>
    /// Return to main menu and destroy this duplicate gameManager
    /// </summary>
    public void ReturnToMenu()
    {
        //load menu scene
        StartCoroutine(LoadScene(0));

        //destroy this object after the menu is reloded
        Destroy(gameObject);
    }


    public void LevelComplete()
    {
        // pause game
        Time.timeScale = 0f;
        AudioListener.pause = false;
        score = playerSpawner.player.combat.killCount; //TODO +1
        LevelCompleteMenu.SetActive(true);
    }

    public void NextLevel()
    {
        StartCoroutine(LoadGameScene(2, true));
        LevelCompleteMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void SaveScore()
    {
        score = playerSpawner.player.combat.killCount;

        //load score scene
        StartCoroutine(LoadScene(3));

        LevelCompleteMenu.SetActive(false);

    }

}

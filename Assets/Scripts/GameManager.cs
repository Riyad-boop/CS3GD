using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private ZombieSpawner enemySpawner;
    private PlayerSpawner playerSpawner;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void NewGame()
    {
        StartCoroutine(LoadGameScene(1,true));

    }

    public void SaveGame()
    {
        enemySpawner.SaveZombies();
        playerSpawner.SavePlayer();
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

        sceneLoader = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + level);
  
        while(!sceneLoader.isDone)
        {
            yield return null;
        }

        //once the scene has loaded then start new game or load game
        // player needs to spawned first to send its transform to other dependant gameobjects
        if (newGame)
        {
            playerSpawner = GameObject.FindGameObjectWithTag("PlayerSpawner").GetComponent<PlayerSpawner>();
            playerSpawner.SpawnNewPlayer(this);
            enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<ZombieSpawner>();
            enemySpawner.NewGameSpawn();
        }
        else
        {
            playerSpawner = GameObject.FindGameObjectWithTag("PlayerSpawner").GetComponent<PlayerSpawner>();
            playerSpawner.LoadPlayer(this);
            enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<ZombieSpawner>();
            enemySpawner.LoadZombies();
        }
    }
    
}

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Rendering.Universal.Internal;
using System.Runtime.Serialization;
using System.Runtime.InteropServices.ComTypes;

public static class SaveLoadSystem
{
    public static List<Zombie> zombies = new List<Zombie>();
    const string PLAYER_FILEPATH = "/player";
    const string ZOMBIE_FILEPATH = "/zombie";
    const string ZOMBIE_COUNT_FILEPATH = "/zombie.count";


    /// <summary>
    /// Saves the player to binary file using a player object
    /// </summary>
    /// <param name="player"></param>
    public static void SavePlayer(Player player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + PLAYER_FILEPATH;
        
        //open filestream in create mode
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);

        //format the file into a binary file and save the data
        formatter.Serialize(stream, data);
        stream.Close();
    }

    /// <summary>
    /// loads the data into a PlayerData instance from a binary file
    /// </summary>
    /// <returns></returns>
    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + PLAYER_FILEPATH;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            //open filestream in open mode
            FileStream stream = new FileStream(path, FileMode.Open);

            //cast the data to playerData type
            PlayerData data =  formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("No save file found in: " + path);
            return null;
        }

    }


    /// <summary>
    /// Saves all the zombies into a file
    /// </summary>
    public static void SaveZombies()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + ZOMBIE_FILEPATH + SceneManager.GetActiveScene().buildIndex;
        string countPath = Application.persistentDataPath + ZOMBIE_COUNT_FILEPATH + SceneManager.GetActiveScene().buildIndex;

        //writing the number of zombies saved in a separate file
        FileStream fileCountStream = new FileStream(countPath, FileMode.Create);
        formatter.Serialize(fileCountStream, zombies.Count);
        fileCountStream.Close();

        // writing each zombie to file
        for (int index=0; index<zombies.Count; index++)
        {
            FileStream stream = new FileStream(path + index, FileMode.Create);
            ZombieData data = new ZombieData(zombies[index]);

            formatter.Serialize(stream, data);
            stream.Close();
        }
     
    }

    public static List<ZombieData> loadZombies()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + ZOMBIE_FILEPATH + SceneManager.GetActiveScene().buildIndex;
        string countPath = Application.persistentDataPath + ZOMBIE_COUNT_FILEPATH + SceneManager.GetActiveScene().buildIndex;
        int zombieCount = 0;
        List<ZombieData> zombiesDataList = new List<ZombieData>();

        if (File.Exists(countPath))
        {
            FileStream fileCountStream = new FileStream(countPath, FileMode.Open);
            //cast the deserialised data to int
            zombieCount = (int) formatter.Deserialize(fileCountStream);
            fileCountStream.Close();
        }
        else
        {
            Debug.LogError("No save file found in: " + countPath);
            return null;
        }

        // load each zombie savefile
        for(int index = 0; index < zombieCount; index++)
        {
             if (File.Exists(path + index))
             {
                FileStream stream = new FileStream(path + index, FileMode.Open);
                ZombieData data = formatter.Deserialize(stream) as ZombieData;
                zombiesDataList.Add(data);
                stream.Close();
             }
             else
             {
                 Debug.LogError("No save file found in: " + path + index);
                 return null;
             }
        }   

        return zombiesDataList;
    }
}

using UnityEngine;
using Firebase.Database;
using TMPro;
using System.Collections;
using System;
using System.Collections.Generic;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using Unity.VisualScripting;

public class DatabaseManager : MonoBehaviour
{
    public TMP_Text score_text;
    public TMP_Text db_username_text;
    public TMP_Text db_score_text;
    public float inputScore;
    private string user_id;

    [SerializeField]
    private string user_name;
    private DatabaseReference dbReference;

    private GameObject gameManagerObject;
    private List<User> userList = new List<User>();
    private UserScoreComparer comparer = new UserScoreComparer();

    [SerializeField]
    private TMP_Text[] HighScores;
    [SerializeField]
    private TMP_Text[] HighScoreUsers;


    private void Awake()
    {
        user_name = PlayerPrefs.GetString("SavedUsername", "Anonymous");
    }
    /// <summary>
    /// set up the database reference and user id
    /// </summary>
    void Start()
    {
        user_id = SystemInfo.deviceUniqueIdentifier;
        db_username_text.text = user_name;
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;

        gameManagerObject = GameObject.Find("GameManager");

        if (gameManagerObject != null)
        {
            SetInputScore(gameManagerObject.GetComponent<GameManager>().score);
        }

        //get leaderboard stats
        StartCoroutine(GetLeaderboardUsers());
       

    }

    /// <summary>
    /// Sorts the scores from the leaderboards
    /// </summary>
    void SortByScore()
    {
        // Sort the list by score using a custom comparer
        userList.Sort(comparer);

        // get slice of first top 5 scoring users.
        userList = userList.GetRange(0, userList.Count);

        int count = 0;
        foreach (User user in userList)
        {
            HighScores[count].text = user.score.ToString();
            HighScoreUsers[count].text = user.username;
            count++;
        }
    }

    /// <summary>
    /// Gets all the users in the leadeboard collection
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    public IEnumerator GetLeaderboardUsers()
    {
        userList = new List<User>();
        var getallUsersTask = dbReference.Child("leaderboards").GetValueAsync();
        yield return new WaitUntil(predicate: () => getallUsersTask.IsCompleted);

        if (getallUsersTask != null)
        {
            DataSnapshot snapshots = getallUsersTask.Result;
            foreach (var data in snapshots.Children)
            {
                User user = new User(data.Child("username").Value.ToString(), float.Parse(data.Child("score").Value.ToString()));
                userList.Add(user);
            }
        }

        SortByScore();
    }

    /// <summary>
    /// Write's user's data to the firebase database
    /// </summary>
    public void WriteUser() {
        User user = new User(user_name, inputScore);
        string json = JsonUtility.ToJson(user);
        dbReference.Child("users").Child(user_id).SetRawJsonValueAsync(json);

        int insertIndex = userList.BinarySearch(user, comparer);
        //check if the score is in the top 5 (0,1,2,3,4), otherwise don't update the leaderboard
        if (insertIndex <= 4)
        {
            dbReference.Child("leaderboards").Child(user_id).SetRawJsonValueAsync(json);
        }
    }

    /// <summary>
    /// asynchronously fetches the user data and creates a user object from it 
    /// </summary>
    /// <param name="callback"> contains the user's data </param>
    /// <returns></returns>
    public IEnumerator GetUser(Action<User> callback)
    {
        var dbTask = dbReference.Child("users").Child(user_id).GetValueAsync();
        yield return new WaitUntil(predicate:() => dbTask.IsCompleted);

        if (dbTask != null)
        {
            DataSnapshot snapshot = dbTask.Result;
            User dbUser = new User(snapshot.Child("username").Value.ToString(), float.Parse(snapshot.Child("score").Value.ToString()));
            callback.Invoke(dbUser);
        }

        StartCoroutine(GetLeaderboardUsers());
    }

    /// <summary>
    /// Starts an asynchronous function to fetch the user's data
    /// </summary>
    public void ReadUser()
    {
        StartCoroutine(GetUser((User user) =>
        {
            db_username_text.text = user.username;
            db_score_text.text = user.score.ToString();
        }));
    }



    public void SetInputScore(int score)
    {
        inputScore = score;
        score_text.text = score.ToString();
    }

    public void ReturnToMenu()
    {
        if (gameManagerObject != null)
        {
            gameManagerObject.GetComponent<GameManager>().ReturnToMenu();
        }
    }
  
}

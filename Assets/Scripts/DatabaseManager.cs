using UnityEngine;
using Firebase.Database;
using TMPro;
using System.Collections;
using System;

public class DatabaseManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_Text score_text;
    public TMP_Text db_username_text;
    public TMP_Text db_score_text;
    public float inputScore;
    private string user_id;
    private DatabaseReference dbReference;

    private GameObject gameManagerObject;

    /// <summary>
    /// set up the database reference and user id
    /// </summary>
    void Start()
    {
        user_id = SystemInfo.deviceUniqueIdentifier;
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;

        gameManagerObject = GameObject.Find("GameManager");

        if (gameManagerObject != null)
        {
            SetInputScore(gameManagerObject.GetComponent<GameManager>().score);
        }

    }

    /// <summary>
    /// Write's user's data to the firebase database
    /// </summary>
    public void WriteUser() {
        User user = new User(usernameInput.text, inputScore);
        string json = JsonUtility.ToJson(user);
        dbReference.Child("users").Child(user_id).SetRawJsonValueAsync(json);
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

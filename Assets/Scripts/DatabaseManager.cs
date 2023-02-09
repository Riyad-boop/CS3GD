using UnityEngine;
using Firebase.Database;
using TMPro;
using System.Collections;
using System;

public class DatabaseManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_Text username_text;
    public TMP_Text score_text;
    public float score;
    private string user_id;
    private DatabaseReference dbReference;

    /// <summary>
    /// set up the database reference and user id
    /// </summary>
    void Start()
    {
        user_id = SystemInfo.deviceUniqueIdentifier;
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    /// <summary>
    /// Write's user's data to the firebase database
    /// </summary>
    public void WriteUser() {
        User user = new User(usernameInput.text, score);
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
            username_text.text = user.username;
            score_text.text = user.score.ToString();
        }));
    }
  
}

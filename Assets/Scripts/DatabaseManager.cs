using UnityEngine;
using Firebase.Database;
using TMPro;

public class DatabaseManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
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

    public void CreateUser() {
        User user = new User(usernameInput.text, score);
        Debug.Log(dbReference.Reference);
        string json = JsonUtility.ToJson(user);
        dbReference.Child("users").Child(user_id).SetRawJsonValueAsync(json);
    }
  
}

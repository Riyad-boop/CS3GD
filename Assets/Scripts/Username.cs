using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Username : MonoBehaviour
{
    private TMP_InputField username;
   
    // Start is called before the first frame update
    private void Start()
    {
        username = GetComponent<TMP_InputField>();
        username.text = PlayerPrefs.GetString("SavedUsername", "Anonymous");
    }

    public void OnInputChange()
    {
        if (username.text != null)
        {
            PlayerPrefs.SetString("SavedUsername", username.text);
        }
    }

    
}

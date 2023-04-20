using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadNextScene : MonoBehaviour
{
    //this must be set in the editor
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(loadFirstLevel());
    }

    IEnumerator loadFirstLevel()
    {
        //wait 34 seconds radio audio to finish
        yield return new WaitForSeconds(34);
        gameManager.LoadFirstLevel();
    }
}

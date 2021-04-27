using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MainLevel");
        //string levelToLoad = PlayerPrefs.GetString("LevelToLoad");
        //if (levelToLoad != null)
        //{
        //    SceneManager.LoadScene(PlayerPrefs.GetString("LevelToLoad"));
        //} else
        //{
        //    Debug.Log("No level to load!");
        //}
    }
}

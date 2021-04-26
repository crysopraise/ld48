using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStartScript : MonoBehaviour
{
    Scene thisScene;

    // Start is called before the first frame update
    void Start()
    {
        thisScene = SceneManager.GetActiveScene();
        PlayerPrefs.SetString("LevelToLoad", thisScene.path);
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

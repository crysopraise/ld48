using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Endgame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy() {
        Invoke("Win", 3);
    }

    void Win() {
        SceneManager.LoadScene("WinScreen");
    }
}

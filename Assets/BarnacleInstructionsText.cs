using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarnacleInstructionsText : MonoBehaviour
{
    GameObject PlayerObject;

    // Start is called before the first frame update
    void Start()
    {
        PlayerObject = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerMovement p = PlayerObject.GetComponent<PlayerMovement>();
            PlayerObject.GetComponent<PlayerMovement>().ShowBarnacleInfoText();
            Destroy(this);
        }
    }
}

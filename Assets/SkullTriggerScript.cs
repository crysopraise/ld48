using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullTriggerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        HarpoonScript h = other.GetComponent<HarpoonScript>();
        if(h)
        {
            if (h.harpoonFlying)
            {
                Debug.Log("Harpoon hit!!!");
                // do stuff
            }
        }
    }
}

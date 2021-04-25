using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarnacleOrbScript : MonoBehaviour
{

    float lifetime = 10;

    void Awake() {
        GetComponent<MeshRenderer>().material.color = Color.magenta;
    }

    void Start() {
        Debug.Log("Barnacle orb instantiated");
    }

    void FixedUpdate()
    {
        lifetime -= Time.fixedDeltaTime;
        if(lifetime < 0)
        {
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player")) {
            // Hurt player
        }
        Destroy(this.gameObject);
    }
}

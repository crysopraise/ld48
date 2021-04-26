using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarnacleOrbScript : MonoBehaviour
{

    float lifetime = 10;
    [SerializeField] GameObject PlayerObject;

    void Awake() {
        GetComponent<MeshRenderer>().material.color = Color.magenta;
    }

    void Start() {
        //Debug.Log("Barnacle orb instantiated");
        PlayerObject = GameObject.Find("Player");

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
            PlayerMovement playerscript = PlayerObject.GetComponent<PlayerMovement>();
            playerscript.Damage(5);
        }
        Destroy(this.gameObject);
    }
}

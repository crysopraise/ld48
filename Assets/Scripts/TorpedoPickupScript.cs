using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorpedoPickupScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().AddTorque(new Vector3(0, 1, 0));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.other.gameObject.CompareTag("Player")) {
            PlayerMovement p = collision.other.gameObject.GetComponent<PlayerMovement>();
            if (p)
            {
                p.AddTorpedos(1);
                Destroy(gameObject);
            }
        }
    }
}

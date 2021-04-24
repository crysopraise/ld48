using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorpedoScript : MonoBehaviour
{
    [SerializeField] GameObject impactPrefab;
    Rigidbody body;

    float armingTime = 0.5f;
    float lifetime;

    // Start is called before the first frame update
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody>();
        lifetime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        // body.velocity += gameObject.transform.forward * Time.fixedDeltaTime * 30.0f;
        body.AddForce(gameObject.transform.forward * Time.fixedDeltaTime * 30.0f);
        lifetime += Time.fixedDeltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (lifetime >= armingTime)
        {
            Destroy(this.gameObject);
            //Instantiate(impactPrefab, this.transform.position, Quaternion.identity);
        }
    }
}

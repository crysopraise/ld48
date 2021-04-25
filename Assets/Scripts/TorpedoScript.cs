using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorpedoScript : MonoBehaviour
{
    [SerializeField] GameObject impactPrefab;
    Rigidbody body;

    float armingTime = 0.25f;
    float lifetime;
    [SerializeField] int damage = 10;

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
        body.AddForce(gameObject.transform.forward * Time.fixedDeltaTime * 90.0f);
        lifetime += Time.fixedDeltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (lifetime >= armingTime)
        {
            Instantiate(impactPrefab, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);

            Collider[] objectsWithinExplosion = Physics.OverlapSphere(transform.position, 30.0f);

            Debug.Log(objectsWithinExplosion.Length);

            foreach (Collider c in objectsWithinExplosion) {
                if (c.gameObject.tag == "Enemy")
                {
                    EnemyHealthScript s = c.gameObject.GetComponent<EnemyHealthScript>();
                    s.Damage(damage);
                }
            }
        }
    }
}

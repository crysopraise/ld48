using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    float lifetime;

    [SerializeField] GameObject impactPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        lifetime = 2.0f;
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
        if(Random.Range(0, 5) != 0)
        {
            Instantiate(impactPrefab, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

}

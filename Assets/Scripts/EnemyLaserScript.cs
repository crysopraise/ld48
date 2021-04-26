using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaserScript : MonoBehaviour
{
    float lifetime;
    int damage = 10;

    [SerializeField] GameObject impactPrefab;
    [SerializeField] GameObject PlayerObject;

    bool destroyThis = false;
    
    // Start is called before the first frame update
    void Start()
    {
        lifetime = 2.0f;
        PlayerObject =  GameObject.Find("Player");
    }

    void FixedUpdate()
    {
        lifetime -= Time.fixedDeltaTime;
        if(lifetime < 0)
        {
            Destroy(this.gameObject);
        }
        if(destroyThis)
        {
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerMovement playerscript = PlayerObject.GetComponent<PlayerMovement>();
            playerscript.Damage(damage);
            destroyThis = true;        }
        if(collision.gameObject.tag == "Terrain") {
            destroyThis = true;
        }
        
    }

}

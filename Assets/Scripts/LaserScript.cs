using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    float lifetime;
    int damage = 1;

    [SerializeField] AudioSource armorSfx;
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
        bool destroyThis = false;
        bool armorImpact = false;

        if(Random.Range(0, 5) != 0)
        {
            destroyThis = true;
        }

        if(collision.gameObject.tag == "Enemy")
        {
            EnemyHealthScript s = collision.gameObject.GetComponent<EnemyHealthScript>();
            s.Damage(damage);
            destroyThis = true;
        }
        if(collision.gameObject.tag == "EnemyHarpoonable")
        {
            EnemyHealthScript s = collision.gameObject.GetComponent<EnemyHealthScript>();
            s.Damage(damage);
            destroyThis = true;
        }
        if(collision.gameObject.CompareTag("BarnacleArmor") || collision.gameObject.CompareTag("ClamArmor"))
        {
            armorImpact = true;
        }

        if(destroyThis)
        {
            GameObject i = Instantiate(impactPrefab, this.transform.position, Quaternion.identity);
            if (armorImpact) i.GetComponent<LaserImpactScript>().armorImpact = true;
            Destroy(this.gameObject);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthScript : MonoBehaviour
{
    [SerializeField] int Health;
    [SerializeField] GameObject deathPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(int damage)
    {
        Health -= damage;
        if (Health < -0)
        {
            Instantiate(deathPrefab, this.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserImpactScript : MonoBehaviour
{
    [SerializeField] float lifetime;
    [SerializeField] AudioSource armorImpactSfx;
    public bool armorImpact = false;

    // Start is called before the first frame update
    void Start()
    {
        if(armorImpact)
        {
            armorImpactSfx.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if(lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
}

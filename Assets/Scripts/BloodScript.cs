using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodScript : MonoBehaviour
{
    [SerializeField] float lifetime;
    public AudioClip deathSound;

    // Start is called before the first frame update
    void Start()
    {
        if (deathSound)
        {
            AudioSource a = gameObject.GetComponent<AudioSource>();
            a.clip = deathSound;
            a.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
}

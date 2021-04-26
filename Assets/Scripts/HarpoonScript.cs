using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonScript : MonoBehaviour
{
    [SerializeField] FixedJoint strikeJoint;
    public bool stuckInTerrain;
    public bool harpoonFlying;

    [SerializeField] AudioClip rockImpactClip;
    [SerializeField] AudioClip fleshImpactClip;
    [SerializeField] AudioClip chitinImpactClip;
    [SerializeField] AudioClip genericImpactClip;

    AudioSource audioSource;

    int damage = 3;

    // Start is called before the first frame update
    void Start()
    {
        strikeJoint = null;
        stuckInTerrain = false;
        harpoonFlying = false;

        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" && harpoonFlying)
        {
            harpoonFlying = false;

            if (other.gameObject.tag == "EnemyArmor")
            {
                ClamShellScript s = other.gameObject.GetComponent<ClamShellScript>();
                s.OnHarpoonHit();
                audioSource.clip = chitinImpactClip;
                audioSource.Play();
            }
            if (other.gameObject.tag == "Enemy")
            {
                EnemyHealthScript s = other.gameObject.GetComponent<EnemyHealthScript>();
                s.Damage(damage);
                audioSource.clip = fleshImpactClip;
                audioSource.Play();
            }

            if (other.gameObject.tag == "EnemyHarpoonable")
            {
                EnemyHealthScript s = other.gameObject.GetComponent<EnemyHealthScript>();
                s.Damage(damage);
                audioSource.clip = fleshImpactClip;
                audioSource.Play();
            }

            gameObject.AddComponent<FixedJoint>();
            strikeJoint = gameObject.GetComponent<FixedJoint>();
            if (other.attachedRigidbody)
            {
                strikeJoint.connectedBody = other.attachedRigidbody;
            }
            else
            {
                audioSource.clip = rockImpactClip;
                audioSource.Play();
                stuckInTerrain = true;
            }
        }
    }

    public void FireHarpoon()
    {
        harpoonFlying = true;
    }

    public void DetachHarpoon()
    {
        harpoonFlying = false;
        stuckInTerrain = false;
        if (strikeJoint)
        {
            DestroyImmediate(strikeJoint);
        }
    }

    public bool EnemyStuck()
    {
        return (strikeJoint && !stuckInTerrain);
    }
}

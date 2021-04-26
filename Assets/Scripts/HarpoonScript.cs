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
            this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

            if (other.gameObject.tag == "ClamArmor")
            {
                //ClamShellScript s = other.gameObject.GetComponent<ClamShellScript>();
                //s.OnHarpoonHit();
                audioSource.clip = chitinImpactClip;
                audioSource.Play();
            }
            if (other.gameObject.tag == "BarnacleArmor")
            {
                //BarnacleShellScript b = other.gameObject.GetComponent<BarnacleShellScript>();
                //b.OnHarpoonHit();
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

            if (other.gameObject.tag == "ClamCollider")
            {
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

            strikeJoint = gameObject.AddComponent<FixedJoint>();
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

    public void RetractHarpoon()
    {
        if(strikeJoint)
        {
            if(strikeJoint.connectedBody)
            {
                if (strikeJoint.connectedBody.CompareTag("BarnacleArmor"))
                {
                    BarnacleShellScript b = strikeJoint.connectedBody.GetComponent<BarnacleShellScript>();
                    b.UnstickFromTerrain();
                }

                if (strikeJoint.connectedBody.CompareTag("ClamArmor") || strikeJoint.connectedBody.CompareTag("Enemy"))
                {
                    DetachHarpoon();
                }
            }
        }
    }

    public void FireHarpoon()
    {
        harpoonFlying = true;
        gameObject.tag = "Untagged";
    }

    public void DetachHarpoon()
    {
        harpoonFlying = false;
        stuckInTerrain = false;
        if (strikeJoint)
        {
            Rigidbody rb = strikeJoint.connectedBody;
            Destroy(strikeJoint);
            if (rb)
            {
                if (rb.gameObject.CompareTag("PuzzleBlock"))
                {
                    rb.velocity = new Vector3(0, 0, 0);
                }
            }
        }
    }

    public bool EnemyStuck()
    {
        return (strikeJoint && !stuckInTerrain);
    }
}

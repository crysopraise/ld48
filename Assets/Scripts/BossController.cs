using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private Animation anim;
    public int BrainsRemaining;
    public GameObject Endbrain;
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (BrainsRemaining <= 0) {
            anim.Play();
            if (Endbrain != null)
            {
                EnemyHealthScript endbrainhealthscript = Endbrain.GetComponent<EnemyHealthScript>();
                endbrainhealthscript.Vulnerable = true;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private Animation anim;
    public int BrainsRemaining;
    public GameObject Endbrain;
    float temptimer;
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (BrainsRemaining <= 0) {
            temptimer = temptimer + Time.fixedDeltaTime;
            if (temptimer < 14.5) {
                anim.Play();
            } else {
                anim.Stop();
            }
            if (Endbrain != null)
            {
                EnemyHealthScript endbrainhealthscript = Endbrain.GetComponent<EnemyHealthScript>();
                endbrainhealthscript.Vulnerable = true;
            }
        }
    }
}

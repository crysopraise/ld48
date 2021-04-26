using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBrainDeath : MonoBehaviour
{
    [SerializeField] GameObject BossObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy() {
        BossController bosscont = BossObject.GetComponent<BossController>();
        bosscont.BrainsRemaining = bosscont.BrainsRemaining - 1;
    }

}

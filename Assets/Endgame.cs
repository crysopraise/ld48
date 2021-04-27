using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Endgame : MonoBehaviour
{
    [SerializeField] GameObject BossRibcage;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy() {
        BossRibcage.GetComponent<BossController>().StartEndDelay();
    }

}

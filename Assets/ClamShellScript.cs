using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClamShellScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHarpoonHit()
    {
        gameObject.transform.parent.GetComponent<Animation>().Stop();
        gameObject.transform.parent.SetParent(null);
    }
}

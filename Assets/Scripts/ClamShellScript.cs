using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClamShellScript : MonoBehaviour
{
    [SerializeField] GameObject ClamBody;

    float lifetime;
    bool toDestroy;

    // Start is called before the first frame update
    void Start()
    {
        toDestroy = false;
        lifetime = 5.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(toDestroy)
        {
            lifetime -= Time.deltaTime;
            if(lifetime < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void OnHarpoonHit()
    {
        gameObject.transform.GetComponent<Animation>().Stop();
        Destroy(gameObject.GetComponent<FixedJoint>());
        toDestroy = true;
    }
}

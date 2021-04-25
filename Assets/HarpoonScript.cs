using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonScript : MonoBehaviour
{
    [SerializeField] FixedJoint strikeJoint;
    public bool stuckInTerrain;
    public bool harpoonFlying;

    // Start is called before the first frame update
    void Start()
    {
        strikeJoint = null;
        stuckInTerrain = false;
        harpoonFlying = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" && harpoonFlying)
        {
            Debug.Log("Harpoon hit!");
            harpoonFlying = false;

            gameObject.AddComponent<FixedJoint>();
            strikeJoint = gameObject.GetComponent<FixedJoint>();
            if (other.attachedRigidbody)
            {
                strikeJoint.connectedBody = other.attachedRigidbody;
            } else
            {
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
        DestroyImmediate(strikeJoint);
    }
}

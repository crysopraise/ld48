using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarnacleShellScript : MonoBehaviour
{
    //[SerializeField] GameObject BarnacleShell;
    [SerializeField] GameObject BarnacleParent;

    FixedJoint TerrainAttachment;
    bool HarpoonConnected = false;

    GameObject attachedHarpoon;

    // Start is called before the first frame update
    void Start()
    {
        TerrainAttachment = gameObject.AddComponent<FixedJoint>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UnstickFromTerrain()
    {
        if (TerrainAttachment != null)
        {
            DestroyImmediate(TerrainAttachment);
            GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * 2000);
            EnemyHealthScript barnacleScript = BarnacleParent.GetComponent<EnemyHealthScript>();
            barnacleScript.Vulnerable = true;
            HarpoonConnected = false;
            if (attachedHarpoon)
            {
                attachedHarpoon.GetComponent<HarpoonScript>().DetachHarpoon();
                attachedHarpoon = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Harpoon")
        {
            HarpoonConnected = true;
            attachedHarpoon = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Harpoon")
        {
            HarpoonConnected = false;
            if (attachedHarpoon)
            {
                attachedHarpoon.GetComponent<HarpoonScript>().DetachHarpoon();
                attachedHarpoon = null;
            }
        }
    }
}
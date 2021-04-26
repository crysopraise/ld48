using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClamShellDetacher : MonoBehaviour
{
    [SerializeField] GameObject ClamShell;
    [SerializeField] GameObject ClamParent;
    [SerializeField] GameObject PhysicsPrefab;
    bool HarpoonConnected = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && HarpoonConnected == true) {
            GameObject newPhysicsObject;
            newPhysicsObject = Instantiate(PhysicsPrefab, ClamShell.transform.position, ClamShell.transform.rotation, null);
            newPhysicsObject.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 1000); 
            Destroy(ClamShell);
            EnemyHealthScript clamscript = ClamParent.GetComponent<EnemyHealthScript>();
            clamscript.Vulnerable = true;
            HarpoonConnected = false;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.name == "Harpoon") {
            HarpoonConnected = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.name == "Harpoon") {
            HarpoonConnected = false;
        }
    }

}

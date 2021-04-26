using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarnacleScript : MonoBehaviour
{
    [SerializeField] GameObject orbPrefab;
    [SerializeField] float BulletSpeed;
    [SerializeField] float AttackDelay;
    [SerializeField] float AttackRange;

    [SerializeField] GameObject Shell;
    [SerializeField] GameObject Terrain;

    float DETECTION_RANGE = 100;

    GameObject player;
    LayerMask wallMask;
    [SerializeField] GameObject firePoint;
    bool canAttack = true;
    Material debugMaterial;

    void Awake() {
        player = GameObject.Find("Player");
        wallMask = LayerMask.GetMask("Wall");
        debugMaterial = GetComponent<MeshRenderer>().material;
        Physics.IgnoreCollision(Terrain.GetComponent<Collider>(), GetComponent<Collider>());    // Note: Don't disable collisions between the shell and the terrain!
    }

    void FixedUpdate()
    {
        Vector3 playerHeading = player.transform.position - firePoint.transform.position;
        // Apparently this is more efficient then calculating the magnitude, so wait until player is actually in range to do that
        if (playerHeading.sqrMagnitude <= DETECTION_RANGE * DETECTION_RANGE) {
            float playerDistance = playerHeading.magnitude;
            Vector3 playerDirection = playerHeading / playerDistance;
            if (!Physics.Raycast(firePoint.transform.position, playerDirection, playerDistance, wallMask) && canAttack) {
                debugMaterial.color = Color.green;

                GameObject orb = Instantiate(orbPrefab, firePoint.transform.position, Quaternion.LookRotation(playerDirection));
                Physics.IgnoreCollision(orb.GetComponent<Collider>(), GetComponent<Collider>());
                Physics.IgnoreCollision(orb.GetComponent<Collider>(), Shell.GetComponent<Collider>());
                orb.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * BulletSpeed, ForceMode.VelocityChange);
                canAttack = false;
                Invoke("ResetAttack", AttackDelay);
            }
        } else {
            debugMaterial.color = Color.white;
        }
    }

    void ResetAttack() {
        canAttack = true;
    }
}

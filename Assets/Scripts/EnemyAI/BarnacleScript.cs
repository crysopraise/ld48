using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarnacleScript : MonoBehaviour
{
    [SerializeField] GameObject orbPrefab;
    [SerializeField] float BulletSpeed;
    [SerializeField] float AttackDelay;
    [SerializeField] float AttackRange;

    float DETECTION_RANGE = 100;

    GameObject player;
    LayerMask wallMask;
    Vector3 firePoint;
    bool canAttack = true;
    Material debugMaterial;

    void Awake() {
        player = GameObject.Find("Player");
        wallMask = LayerMask.GetMask("Wall");
        firePoint = transform.Find("FirePoint").position;
        debugMaterial = GetComponent<MeshRenderer>().material;
    }

    void FixedUpdate()
    {
        Vector3 playerHeading = player.transform.position - firePoint;
        // Apparently this is more efficient then calculating the magnitude, so wait until player is actually in range to do that
        if (playerHeading.sqrMagnitude <= DETECTION_RANGE * DETECTION_RANGE) {
            float playerDistance = playerHeading.magnitude;
            Vector3 playerDirection = playerHeading / playerDistance;
            if (!Physics.Raycast(firePoint, playerDirection, playerDistance, wallMask) && canAttack) {
                debugMaterial.color = Color.green;

                GameObject orb = Instantiate(orbPrefab, firePoint, Quaternion.LookRotation(playerDirection));
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
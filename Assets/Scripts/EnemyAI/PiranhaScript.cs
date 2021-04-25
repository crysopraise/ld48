using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiranhaScript : MonoBehaviour
{
    static float DETECTION_RANGE = 100;
    static float ACCELERATION = 30;
    static float KNOCKBACK = 5;
    static float ATTACK_DELAY = 1.25f;

    GameObject player;
    LayerMask wallMask;
    Rigidbody rb;
    Vector3 lastPlayerDirection = Vector3.zero;
    bool canAttack = true;
    Material debugMaterial;

    void Awake() {
        player = GameObject.Find("Player");
        wallMask = LayerMask.GetMask("Wall");
        rb = GetComponent<Rigidbody>();
        debugMaterial = GetComponent<MeshRenderer>().material;
    }

    void Start()
    {

    }

    void FixedUpdate()
    {
        Debug.Log("Velocity: "+rb.velocity);
        Vector3 playerHeading = player.transform.position - transform.position ;
        float playerDistance = playerHeading.magnitude;
        if (playerDistance <= DETECTION_RANGE) {
            Vector3 playerDirection = playerHeading / playerDistance;
            if (!Physics.Raycast(transform.position, playerDirection, playerDistance, wallMask)) {
                debugMaterial.color = Color.green;

                bool changedDirection = lastPlayerDirection == Vector3.zero || playerDirection != lastPlayerDirection;
                if (changedDirection) {
                    transform.localRotation = Quaternion.LookRotation(playerDirection);
                    lastPlayerDirection = playerDirection;
                    if (canAttack && changedDirection)
                        rb.AddRelativeForce(Vector3.forward * ACCELERATION, ForceMode.Acceleration);
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.CompareTag("Player")) {
            // Damage player

            rb.AddRelativeForce(Vector3.back * KNOCKBACK, ForceMode.VelocityChange);
            canAttack = false;
            Invoke("ResetAttack", ATTACK_DELAY);
        }
    }

    void ResetAttack() {
        canAttack = true;
    }
}

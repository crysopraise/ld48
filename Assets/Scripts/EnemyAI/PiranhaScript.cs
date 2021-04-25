using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiranhaScript : MonoBehaviour
{
    [SerializeField] float Acceleration;
    [SerializeField] float TurnSpeed;
    [SerializeField] float MaxSpeed;
    [SerializeField] float Knockback;
    [SerializeField] float AttackDelay;
    [SerializeField] float FacingMargin;

    float DETECTION_RANGE = 100;

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

    void FixedUpdate()
    {
        Vector3 playerHeading = player.transform.position - transform.position;
        // Apparently this is more efficient then calculating the magnitude, so wait until player is actually in range to do that
        if (playerHeading.sqrMagnitude <= DETECTION_RANGE * DETECTION_RANGE) {
            float playerDistance = playerHeading.magnitude;
            Vector3 playerDirection = playerHeading / playerDistance;
            if (!Physics.Raycast(transform.position, playerDirection, playerDistance, wallMask)) {
                debugMaterial.color = Color.green;

                Vector3 facingDifference = playerDirection - transform.rotation.eulerAngles;
                // bool isFacingPlayer = facingDifference.sqrMagnitude < FacingMargin * FacingMargin;
                float angleDiff = Vector3.Angle(transform.forward, playerHeading);
                bool isFacingPlayer = angleDiff < FacingMargin;
                Debug.Log("Is facing player: "+isFacingPlayer+" - "+ angleDiff);
                Vector3 cross = Vector3.Cross(transform.forward, playerHeading);
                rb.AddRelativeTorque(cross * angleDiff * TurnSpeed, ForceMode.Acceleration);
                if (isFacingPlayer) {
                    rb.AddRelativeForce(Vector3.forward * Acceleration, ForceMode.VelocityChange);
                    rb.velocity = Vector3.ClampMagnitude(rb.velocity, MaxSpeed);
                } else if (canAttack) {
                    // float angleDiff = Vector3.Angle(transform.forward, playerHeading);
                    // Vector3 cross = Vector3.Cross(transform.forward, playerHeading);
                    // rb.AddRelativeTorque(cross * angleDiff * TurnSpeed, ForceMode.Acceleration);
                }
                // bool changedDirection = lastPlayerDirection == Vector3.zero || playerDirection != lastPlayerDirection;
                // Quaternion targetRotation = Quaternion.LookRotation(playerDirection);
                // if (changedDirection) {
                //     transform.localRotation = Quaternion.Lerp(transform.rotation, targetRotation, TurnSpeed);
                //     // transform.localRotation = Quaternion.LookRotation(playerDirection);
                //     lastPlayerDirection = playerDirection;
                //     if (canAttack) {
                //         rb.AddRelativeForce(Vector3.forward * Acceleration, ForceMode.VelocityChange);
                //         rb.velocity = Vector3.ClampMagnitude(rb.velocity, MaxSpeed);
                //         Debug.Log("Piranha velocity: "+rb.velocity);
                //         canAttack = false;
                //         Invoke("ResetAttack", AttackDelay);
                //     }
                // }
            }
        } else {
            debugMaterial.color = Color.white;
        }
    }

    void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.CompareTag("Player")) {
            // Damage player

            rb.AddRelativeForce(Vector3.back * Knockback, ForceMode.VelocityChange);
            // canAttack = false;
            // Invoke("ResetAttack", AttackDelay);
        }
    }

    void ResetAttack() {
        canAttack = true;
    }
}

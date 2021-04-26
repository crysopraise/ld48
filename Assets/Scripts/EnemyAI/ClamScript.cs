using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClamScript : MonoBehaviour
{
    // Editor fields
    [SerializeField] float chargeTime;
    [SerializeField] float laserDuration;
    [SerializeField] float laserRange;
    [SerializeField] float laserRadius;
    [SerializeField] float attackCooldown;
    [SerializeField] float regularTurnSpeed;
    [SerializeField] float chargingTurnSpeed;
    [SerializeField] float shootingTurnSpeed;

    // Variables
    const float DETECTION_RANGE = 100;
    enum AttackState { Idle, AttackingPlayer, ChargingLaser, ShootingLaser};

    GameObject player;
    LayerMask wallMask;
    Rigidbody rb;
    GameObject laserInstance;
    AttackState attackState = AttackState.AttackingPlayer;
    bool canAttack = true;
    List<Animation> animations = new List<Animation>();

    GameObject debugLaser;

    void Awake() {
        player = GameObject.Find("Player");
        wallMask = LayerMask.GetMask("Wall");
        GameObject body = transform.Find("clam2").gameObject;
        rb = body.gameObject.GetComponent<Rigidbody>();
        animations.Add(transform.Find("clam1").gameObject.GetComponent<Animation>());
        animations.Add(transform.Find("clam2").gameObject.GetComponent<Animation>());
        animations.Add(transform.Find("clam3").gameObject.GetComponent<Animation>());
        }
    }

    void FixedUpdate() {
        Vector3 playerHeading = player.transform.position - transform.position;
        float turnSpeed = regularTurnSpeed;

        if (attackState == AttackState.AttackingPlayer) {
            if (canAttack) {
                ChargeLaser();
            }
        }
        if (attackState == AttackState.ChargingLaser) {
            turnSpeed = chargingTurnSpeed;
        }
        if (attackState == AttackState.ShootingLaser) {
            turnSpeed = shootingTurnSpeed;
            RaycastHit hit;
            Physics.SphereCast(transform.position, laserRadius, transform.forward, out hit, laserRange);
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Player")) {
                Debug.Log("player hit! eat it bitch!!");
                // Damage player
            }

            // debug laser
            PositionDebugLaser();
        }

        TurnTowardPlayer(playerHeading, turnSpeed);
    }

    void TurnTowardPlayer(Vector3 playerHeading, float turnSpeed) {
        Quaternion targetRotation = Quaternion.LookRotation(playerHeading) * Quaternion.Euler(new Vector3(0, 90, 0));
        float strength = Mathf.Min(turnSpeed * Time.fixedDeltaTime, 1);
        rb.rotation = Quaternion.Lerp(rb.transform.rotation, targetRotation, strength);
    }

    void ChargeLaser() {
        canAttack = false;
        attackState = AttackState.ChargingLaser;
        Invoke("ShootLaser", chargeTime);
        PlayAttackAnimation();
    }

    void ShootLaser() {
        attackState = AttackState.ShootingLaser;
        // Spawn laser prefab
        // debug laser
        debugLaser = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        debugLaser.GetComponent<Collider>().enabled = false;
        debugLaser.GetComponent<MeshRenderer>().material.color = Color.cyan;
        PositionDebugLaser();

        Invoke("FinishAttack", laserDuration);
    }

    void FinishAttack() {
        // Despawn laser
        Destroy(debugLaser);

        attackState = AttackState.AttackingPlayer;
        Invoke("ResetAttack", attackCooldown);
        Invoke("StopAnimation", 6 - laserDuration - chargeTime);
    }

    void ResetAttack() {
        canAttack = true;
    }

    void PlayAttackAnimation() {
        for (int i = 0; i < 3; i++) {
            animations[i].Play();
        }
    }

    void StopAnimation() {
        for (int i = 0; i < 3; i++) {
            animations[i].Stop();
        }
    }

    void PositionDebugLaser() {
        debugLaser.transform.position = transform.position;
        float scale = 2;
        debugLaser.transform.localScale = new Vector3(scale, laserRange / 2, scale);
        debugLaser.transform.rotation = rb.rotation * Quaternion.Euler(new Vector3(90, -90, 0));
        debugLaser.transform.Translate(new Vector3(0, laserRange / 2, 0));
    }
}

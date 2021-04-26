using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V2ClamScript : MonoBehaviour
{
    float AnimTimer;
    bool fired;
    bool turning;
    Rigidbody rb;
    GameObject player;
    [SerializeField] GameObject LaserFiringPoint;
    [SerializeField] GameObject EnemyLaserPrefab;

    void Start()
    {
        AnimTimer = 0f;
        rb = gameObject.GetComponent<Rigidbody>();
        player = GameObject.Find("Player");

    }

    void FixedUpdate()
    {
        Vector3 playerHeading = player.transform.position - transform.position;
        if (AnimTimer > 4) {
            turning = false;
        }
        if (turning) {
            TurnTowardPlayer(playerHeading, 3);
        }
        if (AnimTimer > 4.68 && fired == false) {
            fired = true;
            //Spawn bullet here
            GameObject newLaser = Instantiate(EnemyLaserPrefab, LaserFiringPoint.transform.position, gameObject.transform.rotation);
            Vector3 firingDirection = gameObject.transform.forward;
            newLaser.GetComponent<Rigidbody>().AddForce(firingDirection.normalized * 1000);
        }
        if (AnimTimer > 5.5) {
            rb.AddRelativeForce(Vector3.forward * 250);
        }
        AnimTimer = AnimTimer + Time.deltaTime;
        if (AnimTimer >= 6) {
            AnimTimer = 0;
            fired = false;
            turning = true;
        }
    }

    void TurnTowardPlayer(Vector3 playerHeading, float turnSpeed) {
        Quaternion targetRotation = Quaternion.LookRotation(playerHeading) * Quaternion.Euler(new Vector3(0, 0, 0));
        float strength = Mathf.Min(turnSpeed * Time.fixedDeltaTime, 1);
        rb.rotation = Quaternion.Lerp(rb.transform.rotation, targetRotation, strength);
    }
}

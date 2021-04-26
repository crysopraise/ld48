using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiranhaScript : MonoBehaviour
{
    [SerializeField] float AttackVelocity;
    [SerializeField] float SwimVelocity;
    [SerializeField] float KnockBackVelocity;
    [SerializeField] float TurnSpeed;
    [SerializeField] float AttackDelay;
    [SerializeField] float FacingMargin;
    [SerializeField] int AttackDamage;
    //[SerializeField] AudioSource attackSoundSource;
    [SerializeField] AudioSource ambientSoundSource;
    [SerializeField] GameObject PlayerObject;


    float DETECTION_RANGE = 100;

    GameObject player;
    LayerMask wallMask;
    Rigidbody rb;
    Vector3 lastPlayerDirection = Vector3.zero;
    bool canAttack = true;
    float ambientSoundTimer;

    void Awake() {
        player = GameObject.Find("Player");
        wallMask = LayerMask.GetMask("Wall");
        rb = GetComponent<Rigidbody>();

        ambientSoundTimer = Random.Range(5f, 10f);
    }

    void Start() {
        PlayerObject =  GameObject.Find("Player");
    }

    void FixedUpdate()
    {
        Vector3 playerHeading = player.transform.position - transform.position;
        // Apparently this is more efficient then calculating the magnitude, so wait until player is actually in range to do that
        if (playerHeading.sqrMagnitude <= DETECTION_RANGE * DETECTION_RANGE) {
            float playerDistance = playerHeading.magnitude;
            Vector3 playerDirection = playerHeading / playerDistance;

            if (!Physics.Raycast(transform.position, playerDirection, playerDistance, wallMask)) {
                Vector3 facingDifference = (playerDirection - transform.rotation.eulerAngles).normalized;
                float angleDiff = Vector3.Angle(transform.forward, playerHeading);
                bool isFacingPlayer = angleDiff < FacingMargin;

                if (isFacingPlayer && canAttack) {
                    rb.velocity = transform.forward * AttackVelocity;
                } else {
                    Quaternion targetRotation = Quaternion.LookRotation(playerHeading);
                    float strength = Mathf.Min(TurnSpeed * Time.fixedDeltaTime, 1);
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, strength);

                    if (canAttack) {
                        rb.velocity = transform.forward * SwimVelocity;
                    }
                }
            }
        }

        ambientSoundTimer -= Time.fixedDeltaTime;
        if(ambientSoundTimer <= 0)
        {
            // Ambient sound range is double detection range
            if (playerHeading.sqrMagnitude <= DETECTION_RANGE * DETECTION_RANGE)
            {
                ambientSoundSource.Play();
                ambientSoundTimer = Random.Range(5f, 10f);
            }
        }
    }

    void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.CompareTag("Player")) {
            // Damage player

            rb.AddRelativeForce(Vector3.back * KnockBackVelocity, ForceMode.VelocityChange);
            if (collision.gameObject.tag == "Player")
            {
                PlayerMovement playerscript = PlayerObject.GetComponent<PlayerMovement>();
                playerscript.Damage(AttackDamage);
            }
            canAttack = false;
            Invoke("ResetAttack", AttackDelay);
        }
    }

    void ResetAttack() {
        canAttack = true;
    }
}

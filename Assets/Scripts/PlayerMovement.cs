using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Vector3 velocity

    [SerializeField] float moveSpeed = 1.0f;
    [SerializeField] float turnSpeed = 1.0f;
    [SerializeField] float rollSpeed = 1.0f;
    [SerializeField] bool invertY = false;

    [SerializeField] GameObject laserPrefab;

    [SerializeField] float laserBaseFireRate = 1.0f;
    [SerializeField] float laserOverheatedFireRate = 1.0f;

    [SerializeField] float laserBaseSpreadRadius = 0.02f;
    [SerializeField] float laserOverheatedSpreadRadius = 0.02f;

    // [SerializeField] float laserHeatRate = 1.0f;

    float laserShotTimer;

    float laserHeat;
    [SerializeField] float laserOverheatThreshold;  // Number of seconds of continuous fire to overheat the laser
    [SerializeField] float laserCooldownTime; // How long it should take to cool down after overheating

    bool laserIsOverheated;

    [SerializeField] GameObject torpedoPrefab;

    [SerializeField] float torpedoFireRate = 1.0f;
    float torpedoShotTimer;

    Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
        // velocity = new Vector3(0, 0, 0);
        body = this.GetComponent<Rigidbody>();

        laserShotTimer = 0;
        laserHeat = 0;
        laserIsOverheated = false;
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        body.AddRelativeTorque(GetRotation(), ForceMode.VelocityChange);
        body.AddRelativeForce(GetDirection() * moveSpeed, ForceMode.VelocityChange);

        if (laserShotTimer >= 0)
        {
            laserShotTimer -= Time.fixedDeltaTime;
        } else
        {
            //if (Input.GetKey(KeyCode.Space) && !laserIsOverheated)
            if (Input.GetMouseButton(0) && !laserIsOverheated)
            {
                FireLaser();
                if(laserHeat > laserOverheatThreshold)
                {
                    laserIsOverheated = true;
                }
            } else
            {
                if (laserHeat > 0) {
                    laserHeat -= Time.fixedDeltaTime * laserOverheatThreshold / laserCooldownTime;
                } else
                {
                    laserHeat = 0;
                    laserIsOverheated = false;
                }
            }
        }

        if (torpedoShotTimer >= 0)
        {
            torpedoShotTimer -= Time.fixedDeltaTime;
        }
        else
        {
            if (Input.GetMouseButton(1))
            {
                FireTorpedo();
            }
        }
    }

    Vector3 GetDirection()
    {
        // Create a movement direction vector based on keyboard input.
        var dir = new Vector3();
        if (Input.GetKey(KeyCode.W)) dir += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) dir += Vector3.back;
        if (Input.GetKey(KeyCode.A)) dir += Vector3.left;
        if (Input.GetKey(KeyCode.D)) dir += Vector3.right;
        if (Input.GetKey(KeyCode.LeftControl)) dir += Vector3.down;
        if (Input.GetKey(KeyCode.LeftShift)) dir += Vector3.up;
        return dir;
    }

    Vector3 GetRotation()
    {
        float yaw = Input.GetAxis("Mouse X");
        float pitch = Input.GetAxis("Mouse Y") * (invertY ? 1 : -1);
        float roll = 0;
        if (Input.GetKey(KeyCode.Q)) roll += 1;
        if (Input.GetKey(KeyCode.E)) roll -= 1;
        return new Vector3(pitch * turnSpeed, yaw * turnSpeed, roll * rollSpeed);
    }

    private Vector3 FiringPoint()   // The point that weapons are fired from
    {
        //return gameObject.transform.position - gameObject.transform.up * 0.25f + gameObject.transform.forward * 0.65f;
        return gameObject.transform.position - gameObject.transform.up * 0.25f;
    }

    private float LaserHeatPercentage()
    {
        return laserHeat / laserOverheatThreshold;
    }

    private float LaserFireRate() {
        return laserBaseFireRate * (1 - LaserHeatPercentage()) + laserOverheatedFireRate * LaserHeatPercentage();
    }

    private float LaserSpreadRadius()
    {
        return laserBaseSpreadRadius * (1 - LaserHeatPercentage()) + laserOverheatedSpreadRadius * LaserHeatPercentage();
    }

    private void FireLaser()
    {
        GameObject newLaser = Instantiate(laserPrefab, FiringPoint(), gameObject.transform.rotation);
        Physics.IgnoreCollision(newLaser.GetComponent<Collider>(), GetComponent<Collider>());

        Vector3 firingDirection = gameObject.transform.forward + Random.insideUnitSphere * LaserSpreadRadius();

        // newLaser.GetComponent<Rigidbody>().velocity = firingDirection.normalized * 50.0f + body.velocity;
        newLaser.GetComponent<Rigidbody>().velocity = body.velocity;
        newLaser.GetComponent<Rigidbody>().AddForce(firingDirection.normalized * 50.0f);
        laserShotTimer = LaserFireRate();
        laserHeat += LaserFireRate();

        Debug.Log(laserHeat);
    }

    private void FireTorpedo()
    {
        GameObject newTorpedo = Instantiate(torpedoPrefab, FiringPoint(), gameObject.transform.rotation);
        Physics.IgnoreCollision(newTorpedo.GetComponent<Collider>(), GetComponent<Collider>());

        Vector3 firingDirection = gameObject.transform.forward;

        // newTorpedo.GetComponent<Rigidbody>().velocity = firingDirection.normalized * 0.5f + body.velocity;
        newTorpedo.GetComponent<Rigidbody>().velocity = body.velocity;
        newTorpedo.GetComponent<Rigidbody>().AddForce(firingDirection.normalized * 0.5f);
        torpedoShotTimer = torpedoFireRate;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    bool laserBarrelAlternate;   // lasers alternate between being fired from left and right barrels

    [SerializeField] GameObject torpedoPrefab;

    [SerializeField] float torpedoFireRate = 1.0f;
    float torpedoShotTimer;

    [SerializeField] int torpedoLimit;
    int torpedoCount;

    Rigidbody body;

    [SerializeField] GameObject laserTextObject;
    Text laserText;
    [SerializeField] GameObject laserHeatTextObject;
    Text laserHeatText;
    [SerializeField] GameObject torpedoTextObject;
    Text torpedoText;
    [SerializeField] GameObject torpedoCountTextObject;
    Text torpedoCountText;

    // Start is called before the first frame update
    void Start()
    {
        // velocity = new Vector3(0, 0, 0);
        body = this.GetComponent<Rigidbody>();

        laserText = laserTextObject.GetComponent<Text>();
        laserHeatText = laserHeatTextObject.GetComponent<Text>();
        torpedoText = torpedoTextObject.GetComponent<Text>();
        torpedoCountText = torpedoCountTextObject.GetComponent<Text>();

        laserShotTimer = 0;
        laserHeat = 0;
        laserIsOverheated = false;
        laserBarrelAlternate = false;

        torpedoCount = torpedoLimit;
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
            laserHeat += Time.fixedDeltaTime;
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

        UpdateUIText();
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

    private void UpdateUIText()
    {
        float laserHeatPercentage = laserHeat / laserOverheatThreshold * 100.0f;
        laserHeatText.text = (int)laserHeatPercentage + "%";
        if (laserIsOverheated)
        {
            laserText.color = Color.red;
            laserHeatText.color = Color.red;
        }
        else if(laserHeatPercentage > 70.0f)
        {
            laserText.color = Color.yellow;
            laserHeatText.color = Color.yellow;
        } else
        {
            laserText.color = Color.gray;
            laserHeatText.color = Color.gray;
        }

        torpedoCountText.text = torpedoCount.ToString();
        if(torpedoCount <= 0)
        {
            torpedoText.color = Color.red;
            torpedoCountText.color = Color.red;
        } else if (torpedoCount <= 3)
        {
            torpedoText.color = Color.yellow;
            torpedoCountText.color = Color.yellow;
        } else
        {
            torpedoText.color = Color.gray;
            torpedoCountText.color = Color.gray;
        }
    }

    private Vector3 LaserFiringPoint()   // The point that weapons are fired from
    {
        if (laserBarrelAlternate)
        {
            laserBarrelAlternate = false;
            return gameObject.transform.position - gameObject.transform.up * 0.2f + gameObject.transform.right * 0.66f + gameObject.transform.forward * 0.04f;
        } else
        {
            laserBarrelAlternate = true;
            return gameObject.transform.position - gameObject.transform.up * 0.2f - gameObject.transform.right * 0.66f + gameObject.transform.forward * 0.04f;
        }
    }

    private Vector3 TorpedoFiringPoint()   // The point that weapons are fired from
    {
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
        GameObject newLaser = Instantiate(laserPrefab, LaserFiringPoint(), gameObject.transform.rotation);
        Physics.IgnoreCollision(newLaser.GetComponent<Collider>(), GetComponent<Collider>());

        Vector3 firingDirection = gameObject.transform.forward + Random.insideUnitSphere * LaserSpreadRadius();

        //newLaser.GetComponent<Rigidbody>().velocity = body.velocity;
        newLaser.GetComponent<Rigidbody>().AddForce(firingDirection.normalized * 50.0f);
        laserShotTimer = LaserFireRate();
        //laserHeat += LaserFireRate();
    }

    private void FireTorpedo()
    {
        if (torpedoCount >= 1)
        {
            GameObject newTorpedo = Instantiate(torpedoPrefab, TorpedoFiringPoint(), gameObject.transform.rotation);
            Physics.IgnoreCollision(newTorpedo.GetComponent<Collider>(), GetComponent<Collider>());

            Vector3 firingDirection = gameObject.transform.forward;

            //newTorpedo.GetComponent<Rigidbody>().velocity = body.velocity;
            newTorpedo.GetComponent<Rigidbody>().AddForce(firingDirection.normalized * 0.5f);
            torpedoShotTimer = torpedoFireRate;

            torpedoCount -= 1;
        } else
        {

        }
    }
}

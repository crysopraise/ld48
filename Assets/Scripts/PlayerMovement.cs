using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    // movement code taken from http://wiki.unity3d.com/index.php/SixDPhysicsController

    [SerializeField] GameObject torpedoPrefab;
    [SerializeField] GameObject laserPrefab;

    [SerializeField] float moveSpeed = 1.0f;
    [SerializeField] float turnSpeed = 1.0f;
    [SerializeField] float rollSpeed = 1.0f;
    [SerializeField] bool invertY = false;

    bool lockControls = false;

    //[SerializeField] float laserDamage = 1.0f;
    //[SerializeField] float torpedoDamage = 10.0f;
    //[SerializeField] float harpoonDamage = 3.0f;

    [SerializeField] int maxHealth;
    int health;
    [SerializeField] float healthRegenDelay;
    [SerializeField] int healthRegenRate;
    [SerializeField] GameObject deathScreenFadeObject;
    Image deathScreenFadeImage;
    bool dying = false;
    [SerializeField] float deathTime;
    [SerializeField] float deathScreenFadeTime;
    [SerializeField] float deathImmediateScreenTint;

    [SerializeField] float LaserTravelSpeed = 100f;

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

    [SerializeField] float torpedoFireRate = 1.0f;
    float torpedoShotTimer;

    [SerializeField] int torpedoLimit;
    int torpedoCount;

    Rigidbody body;

    [SerializeField] GameObject shipboard;

    [SerializeField] float harpoonDrag;

    [SerializeField] GameObject harpoon;

    [SerializeField] float harpoonRopeMaxLength;
    float harpoonRopeLength;

    [SerializeField] float harpoonReelSpeed;
    [SerializeField] float harpoonStuckReelSpeed;
    [SerializeField] float harpoonReelDelay;    // The time you must wait after firing the harpoon before starting to reel it in
    float harpoonReelTimer;

    bool harpoonReeling;

    Rigidbody harpoonBody;
    FixedJoint harpoonJoint;
    Vector3 harpoonAttachmentPoint;
    Quaternion harpoonAttachmentRotation;
    bool harpoonAttached;
    ConfigurableJoint harpoonRopeJoint;

    LineRenderer harpoonRopeRenderer;

    HarpoonScript harpoonScript;

    [SerializeField] GameObject laserTextObject;
    Text laserText;
    [SerializeField] GameObject laserHeatTextObject;
    Text laserHeatText;
    [SerializeField] GameObject torpedoTextObject;
    Text torpedoText;
    [SerializeField] GameObject torpedoCountTextObject;
    Text torpedoCountText;
    [SerializeField] GameObject hullTextObject;
    Text hullText;
    [SerializeField] GameObject hullPercentageTextObject;
    Text hullPercentageText;
    [SerializeField] GameObject harpoonTextObject;
    Text harpoonText;

    [SerializeField] GameObject instructionsTextCanvas;

    [SerializeField] AudioSource damageAudioSource;
    [SerializeField] AudioSource deathAudioSource;
    [SerializeField] AudioSource harpoonLaunchAudioSource;
    [SerializeField] AudioSource harpoonReelAudioSource;
    [SerializeField] AudioSource shipDingAudioSource;
    [SerializeField] AudioSource healthPickupAudioSource;
    [SerializeField] AudioSource torpedoPickupAudioSource;
    //[SerializeField] AudioClip damageSoundClip;
    //[SerializeField] AudioClip deathSoundClip;
    //[SerializeField] AudioClip harpoonLaunchClip;
    //[SerializeField] AudioClip harpoonReelClip;
    //[SerializeField] AudioClip shipDingClip;

    // Start is called before the first frame update
    void Start()
    {
        // velocity = new Vector3(0, 0, 0);
        body = this.GetComponent<Rigidbody>();

        laserText = laserTextObject.GetComponent<Text>();
        laserHeatText = laserHeatTextObject.GetComponent<Text>();
        torpedoText = torpedoTextObject.GetComponent<Text>();
        torpedoCountText = torpedoCountTextObject.GetComponent<Text>();
        hullText = hullTextObject.GetComponent<Text>();
        hullPercentageText = hullPercentageTextObject.GetComponent<Text>();
        harpoonText = harpoonTextObject.GetComponent<Text>();

        laserShotTimer = 0;
        laserHeat = 0;
        laserIsOverheated = false;
        laserBarrelAlternate = false;

        health = maxHealth;

        torpedoCount = torpedoLimit;

        harpoonBody = harpoon.GetComponent<Rigidbody>();

        harpoonAttachmentPoint = gameObject.transform.InverseTransformPoint(harpoon.transform.position);
        harpoonAttachmentRotation = Quaternion.Inverse(gameObject.transform.rotation) * harpoon.transform.rotation;

        harpoonRopeJoint = harpoon.GetComponent<ConfigurableJoint>();

        harpoonScript = harpoon.GetComponent<HarpoonScript>();

        harpoonRopeLength = harpoonRopeMaxLength;

        harpoonRopeRenderer = gameObject.GetComponent<LineRenderer>();

        deathScreenFadeImage = deathScreenFadeObject.GetComponent<Image>();

        harpoonReeling = false;

        ////AudioSource[] audioSources = gameObject.GetComponents<AudioSource>();
        ////damageAudioSource = audioSources[0];
        //damageAudioSource = gameObject.AddComponent<AudioSource>();
        //damageAudioSource.clip = damageSoundClip;
        //damageAudioSource.loop = false;

        //deathAudioSource = gameObject.AddComponent<AudioSource>();
        //deathAudioSource.clip = deathSoundClip;
        //deathAudioSource.loop = false;


        ////harpoonLaunchAudioSource = audioSources[1];
        //harpoonLaunchAudioSource = gameObject.AddComponent<AudioSource>();
        //harpoonLaunchAudioSource.clip = harpoonLaunchClip;
        //harpoonLaunchAudioSource.loop = false;

        //// harpoonReelAudioSource = audioSources[2];
        //harpoonReelAudioSource = gameObject.AddComponent<AudioSource>();
        //harpoonReelAudioSource.clip = harpoonReelClip;
        //harpoonReelAudioSource.loop = true;

        //shipDingAudioSource = gameObject.AddComponent<AudioSource>();
        //shipDingAudioSource.clip = shipDingClip;
        //shipDingAudioSource.loop = false;

        ReloadHarpoon();
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.K)) {
            Damage(1000);
        }

        body.AddRelativeTorque(GetRotation(), ForceMode.VelocityChange);
        body.AddRelativeForce(GetDirection() * moveSpeed, ForceMode.VelocityChange);

        if (laserShotTimer >= 0)
        {
            laserShotTimer -= Time.fixedDeltaTime;
            laserHeat += Time.fixedDeltaTime;
        } else
        {
            if (Input.GetMouseButton(0) && !laserIsOverheated && !lockControls)
            {
                FireLaser();
                if (laserHeat > laserOverheatThreshold)
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

        // torpedo firing script
        if (torpedoShotTimer >= 0)
        {
            torpedoShotTimer -= Time.fixedDeltaTime;
        }
        else
        {
            if (Input.GetMouseButton(1) && !lockControls)
            {
                FireTorpedo();
            }
        }

        if (harpoonReelTimer >= 0)
        {
            harpoonReelTimer -= Time.fixedDeltaTime;
        }

        // reel in harpoon
        if (Input.GetKey(KeyCode.Space) && harpoonReelTimer <= 0 && !harpoonAttached && !lockControls && !harpoonReeling)
        {
            harpoonReeling = true;
            harpoonScript.RetractHarpoon();
        }

        if(harpoonReeling && harpoonReelTimer <= 0 && !harpoonAttached)
        {
            if (harpoonScript.EnemyStuck())
            {
                //harpoonBody.velocity = (gameObject.transform.TransformPoint(harpoonAttachmentPoint) - harpoon.transform.position).normalized * harpoonStuckReelSpeed;
                harpoonBody.AddForce((gameObject.transform.TransformPoint(harpoonAttachmentPoint) - harpoon.transform.position).normalized * harpoonStuckReelSpeed * 10.0f * Time.fixedDeltaTime);
            }
            else
            {
                //harpoonBody.velocity = (gameObject.transform.TransformPoint(harpoonAttachmentPoint) - harpoon.transform.position).normalized * harpoonReelSpeed;
                harpoonBody.AddForce((gameObject.transform.TransformPoint(harpoonAttachmentPoint) - harpoon.transform.position).normalized * harpoonReelSpeed * 10.0f * Time.fixedDeltaTime);
            }

            harpoonRopeLength -= harpoonReelSpeed * Time.fixedDeltaTime;

            float harpoonDistance = (harpoonAttachmentPoint - gameObject.transform.InverseTransformPoint(harpoon.transform.position)).magnitude;
            if (harpoonRopeLength > harpoonDistance + 0.1f)
            {
                harpoonRopeLength = harpoonDistance;
            }

            if (harpoonRopeLength < 5.0f || harpoonScript.stuckInTerrain)
            {
                harpoonScript.DetachHarpoon();
            }

            if (harpoonRopeLength <= 0.3f)
            {
                ReloadHarpoon();
            }
            else
            {
                SoftJointLimit newLimit = harpoonRopeJoint.linearLimit;
                newLimit.limit = harpoonRopeLength;
                harpoonRopeJoint.linearLimit = newLimit;
            }

            if (!harpoonReelAudioSource.isPlaying)
            {
                harpoonReelAudioSource.Play();
            }
        }
        else
        {
            harpoonReelAudioSource.Pause();
        }

        UpdateUIText();
    }

    private void Update()
    {
        // This code has to be in Update because GetKeyDown doesn't work with FixedUpdate
        if (Input.GetKeyDown(KeyCode.Space) && !lockControls)
        {
            if (harpoonAttached)
            {
                FireHarpoon();
            }
        }

        harpoonRopeRenderer.SetPositions(new Vector3[] { gameObject.transform.TransformPoint(harpoonAttachmentPoint), harpoon.transform.position });

        if(dying)
        {
            Color c = deathScreenFadeImage.color;
            c.a += (1.0f - deathImmediateScreenTint) * Time.deltaTime / deathScreenFadeTime;
            if (c.a > 1.0f) c.a = 1.0f;
            deathScreenFadeImage.color = c;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.relativeVelocity.magnitude >= 10.0f && collision.gameObject.CompareTag("Terrain") && !shipDingAudioSource.isPlaying)
        {
            shipDingAudioSource.Play();
        }
    }

    Vector3 GetDirection()
    {
        // Create a movement direction vector based on keyboard input.
        var dir = new Vector3();
        if (!lockControls)
        {
            if (Input.GetKey(KeyCode.W)) dir += Vector3.forward;
            if (Input.GetKey(KeyCode.S)) dir += Vector3.back;
            if (Input.GetKey(KeyCode.A)) dir += Vector3.left;
            if (Input.GetKey(KeyCode.D)) dir += Vector3.right;
            if (Input.GetKey(KeyCode.LeftControl)) dir += Vector3.down;
            if (Input.GetKey(KeyCode.LeftShift)) dir += Vector3.up;
        }
        return dir;
    }

    Vector3 GetRotation()
    {
        float yaw = Input.GetAxis("Mouse X");
        float pitch = Input.GetAxis("Mouse Y") * (invertY ? 1 : -1);
        float roll = 0;
        if (Input.GetKey(KeyCode.Q)) roll += 1;
        if (Input.GetKey(KeyCode.E)) roll -= 1;
        if (lockControls)
        {
            yaw = 0;
            pitch = 0;
            roll = 0;
        }
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

        hullPercentageText.text = health + "%";
        if (health <= 0)
        {
            hullPercentageText.text = "0%";
            hullText.color = new Color(0.1f, 0, 0, 1);  // dark red
            hullPercentageText.color = new Color(0.1f, 0, 0, 1);
        }
        else if (health <= 20)
        {
            hullText.color = Color.red;
            hullPercentageText.color = Color.red;
        }
        else if (health <= 50)
        {
            hullText.color = Color.yellow;
            hullPercentageText.color = Color.yellow;
        }
        else
        {
            hullText.color = Color.gray;
            hullPercentageText.color = Color.gray;
        }

        if(harpoonAttached)
        {
            harpoonText.text = "Harpoon Ready";
            harpoonText.color = Color.gray;
        } else if(harpoonReeling)
        {
            harpoonText.text = "Harpoon Reeling In";
            harpoonText.color = Color.yellow;
        } else 
        {
            harpoonText.text = "Harpoon Fired";
            harpoonText.color = Color.red;
        }
        
    }

    private Vector3 LaserFiringPoint()   // Get location to fire laser projectile from
    {
        if (laserBarrelAlternate)   // Alternates between firing from the left and right barrels
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
        Physics.IgnoreCollision(newLaser.GetComponent<Collider>(), harpoon.GetComponent<MeshCollider>());
        Physics.IgnoreCollision(newLaser.GetComponent<Collider>(), harpoon.GetComponent<SphereCollider>());

        Vector3 firingDirection = gameObject.transform.forward + Random.insideUnitSphere * LaserSpreadRadius();

        newLaser.GetComponent<Rigidbody>().AddForce(firingDirection.normalized * LaserTravelSpeed);
        laserShotTimer = LaserFireRate();
    }

    private void FireTorpedo()
    {
        if (torpedoCount >= 1)
        {
            GameObject newTorpedo = Instantiate(torpedoPrefab, TorpedoFiringPoint(), gameObject.transform.rotation);
            Physics.IgnoreCollision(newTorpedo.GetComponent<Collider>(), GetComponent<Collider>());
            Physics.IgnoreCollision(newTorpedo.GetComponent<Collider>(), harpoon.GetComponent<MeshCollider>());
            Physics.IgnoreCollision(newTorpedo.GetComponent<Collider>(), harpoon.GetComponent<SphereCollider>());

            Vector3 firingDirection = gameObject.transform.forward;

            newTorpedo.GetComponent<Rigidbody>().AddForce(firingDirection.normalized * 0.5f);
            torpedoShotTimer = torpedoFireRate;

            torpedoCount -= 1;
        } else
        {

        }
    }

    private void FireHarpoon()
    {
        harpoonReelTimer = harpoonReelDelay;

        harpoonLaunchAudioSource.Play();

        Destroy(harpoonJoint);
        harpoonAttached = false;
        harpoonBody.velocity = new Vector3(0, 0, 0);
        harpoonBody.AddForce(harpoon.transform.forward.normalized * 10000f);
        harpoonBody.drag = harpoonDrag;
        harpoonBody.angularDrag = harpoonDrag;
        harpoonScript.FireHarpoon();
    }

    private void ReloadHarpoon()
    {
        harpoonScript.DetachHarpoon();

        harpoon.tag = "Player";

        harpoon.transform.position = gameObject.transform.position + gameObject.transform.rotation * harpoonAttachmentPoint;
        harpoon.transform.rotation = gameObject.transform.rotation * harpoonAttachmentRotation;
        harpoonBody.velocity = body.velocity;
        harpoonBody.drag = 0;

        harpoon.AddComponent<FixedJoint>();
        harpoonJoint = harpoon.GetComponent<FixedJoint>();
        harpoonJoint.connectedBody = body;

        harpoonReeling = false;
        harpoonAttached = true;

        // Reset harpoon rope length
        harpoonRopeLength = harpoonRopeMaxLength;

        SoftJointLimit newLimit = harpoonRopeJoint.linearLimit;
        newLimit.limit = harpoonRopeLength;
        harpoonRopeJoint.linearLimit = newLimit;
    }

    public void Damage(int d)
    {
        health -= d;
        if(health <= 0)
        {
            if(!dying) PlayerDie();
        } else
        {
            damageAudioSource.Play();
        }
    }
    
    void PlayerDie()
    {
        // game over!
        dying = true;
        deathAudioSource.Play();
        lockControls = true;
        
        Color c = deathScreenFadeImage.color;
        c.a = deathImmediateScreenTint;
        deathScreenFadeImage.color = c;

        Invoke("GameOver", deathTime);
    }

    void GameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("GameOver");
    }

    public int GetHealth()
    {
        return health;
    }

    public void RestoreHealth(int h)
    {
        healthPickupAudioSource.Play();
        health += h;
        if(health > 100)
        {
            health = 100;
        }
    }

    public void AddTorpedos(int t)
    {
        torpedoPickupAudioSource.Play();
        torpedoCount += t;
    }

    public void ShowBarnacleInfoText()
    {
        instructionsTextCanvas.GetComponent<InstructionsHideScript>().ShowBarnacleInfo();
    }
}

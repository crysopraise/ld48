using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Vector3 velocity

    [SerializeField] float moveSpeed = 0.2f;
    [SerializeField] float turnSpeed = 0.5f;
    [SerializeField] float rollSpeed = 0.2f;
    [SerializeField] bool invertY = false;

    Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
        // velocity = new Vector3(0, 0, 0);
        body = this.GetComponent<Rigidbody>();
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
        if ((Input.GetKey(KeyCode.Space)) || (Input.GetKey(KeyCode.LeftShift))) dir += Vector3.up;
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
}

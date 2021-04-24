using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCameraController : MonoBehaviour
{
    float MoveSpeed = 0.5f;
    float LookSpeed = 4f;
    float CameraX;
    float CameraY;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var dir = new Vector3 ();
        if (Input.GetKey (KeyCode.W)) dir += Vector3.forward;
        if (Input.GetKey (KeyCode.S)) dir += Vector3.back;
        if (Input.GetKey (KeyCode.A)) dir += Vector3.left;
        if (Input.GetKey (KeyCode.D)) dir += Vector3.right;
        if (Input.GetKey (KeyCode.LeftShift)) dir += Vector3.down;
        if (Input.GetKey (KeyCode.Space)) dir += Vector3.up;
        transform.position += dir * MoveSpeed;

        if (Input.GetButton("Fire2")) {
            CameraX += Input.GetAxis("Mouse X") * LookSpeed;
            CameraY += Input.GetAxis("Mouse Y") * LookSpeed;
            transform.localRotation = Quaternion.Euler(-CameraY, CameraX, 0);
        }
        // transform.localRotation = Quaternion.Euler(0, CameraY, 0);
        // rb.AddRelativeTorque(transform.up * Input.GetAxis("Horizontal") * 50);
    }
}

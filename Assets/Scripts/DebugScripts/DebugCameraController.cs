using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCameraController : MonoBehaviour
{
    float MoveSpeed = 0.5f;
    float CameraX = 0f;
    float CameraY = 0f;

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
        if (Input.GetKey (KeyCode.LeftControl)) dir += Vector3.down;
        if (Input.GetKey (KeyCode.Space)) dir += Vector3.up;
        transform.position += dir * MoveSpeed;

        CameraX = CameraX + Input.GetAxis("Vertical");
        CameraY = CameraY + Input.GetAxis("Horizontal");
        transform.localRotation = Quaternion.Euler(CameraX, 0, 0);
        // transform.localRotation = Quaternion.Euler(0, CameraY, 0);
        // rb.AddRelativeTorque(transform.up * Input.GetAxis("Horizontal") * 50);
    }
}

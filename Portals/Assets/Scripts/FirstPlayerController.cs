using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FirstPlayerController : MonoBehaviour {

    Camera playerCamera;
    Rigidbody rb;

    public float moveSpeed;
    public Vector2 lookSpeed;

    private float pitch = 0f;
    private float yaw = 0f;

    void Start() {
        playerCamera = GetComponentInChildren<Camera>();
        rb = GetComponent<Rigidbody>();
    }

    void Update() {

        //Move player
        Vector2 inp = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized*moveSpeed;

        rb.velocity = playerCamera.transform.TransformDirection(new Vector3(inp.x,0f,inp.y));


        //rotate camera based on mouse
        yaw += lookSpeed.x * Input.GetAxis("Mouse X");
        pitch -= lookSpeed.y * Input.GetAxis("Mouse Y");

        playerCamera.transform.eulerAngles = new Vector3(pitch, yaw, 0f);

    }
}

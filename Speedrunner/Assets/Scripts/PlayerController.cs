using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    public float moveSpeed;
    public Vector2 lookSpeed;

    //camera stuff
    private float pitch = 0f;
    private float yaw = 0f;

    Rigidbody rb;
    Camera playerCamera;

    void Start() {
        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();
    }


    void Update() {

        Vector2 usin = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        //rb.velocity = new Vector3(transform.forward.x*usin.x,0,transform.forward.z*usin.y)*Time.deltaTime;

        //transform.position += new Vector3(playerCamera.transform.forward.x * usin.x, 0, playerCamera.transform.forward.z * usin.y) * Time.deltaTime;
        transform.position += playerCamera.transform.forward * usin.y * moveSpeed * Time.deltaTime;
        transform.position += playerCamera.transform.right * usin.x * moveSpeed * Time.deltaTime;

        //Camera
        yaw += lookSpeed.x * Input.GetAxis("Mouse X");
        pitch -= lookSpeed.y * Input.GetAxis("Mouse Y");

        playerCamera.transform.eulerAngles = new Vector3(pitch, yaw, 0f);

    }
}

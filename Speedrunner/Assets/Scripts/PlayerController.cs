using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    public float moveSpeed;
    public float rotateSpeed; //prob make instantenous

    public float walkMaxSpeed;
    public float maxSpeed;
    public float sprintMaxSpeed;

    public float jumpSpeed;
    public float maxJumpHoldTime;
    Timer jumpTimer;
    bool isOnGround;

    Rigidbody rb;
    Camera playerCamera;


    void Start() {
        rb = GetComponent<Rigidbody>();
        playerCamera = Camera.main;
    }


    void FixedUpdate() {
        var inp = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        rb.velocity += Vector3.ProjectOnPlane(playerCamera.transform.forward, Vector3.up).normalized * inp.y * moveSpeed * Time.deltaTime;
        rb.velocity += Vector3.ProjectOnPlane(playerCamera.transform.right, Vector3.up).normalized * inp.x * moveSpeed * Time.deltaTime;

        //limit velocity
        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), rb.velocity.y, Mathf.Clamp(rb.velocity.z, -maxSpeed, maxSpeed));

    }

    private void Update() {


    }

    private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("floor")) {
            isOnGround = true;
        }
    }

}

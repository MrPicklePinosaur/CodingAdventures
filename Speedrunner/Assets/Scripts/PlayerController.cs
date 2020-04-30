using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    public float moveSpeed;
    public float walkMaxSpeed;
    public float maxSpeed;
    public float sprintMaxSpeed;

    public float rotateSpeed;

    public Vector2 lookSpeed;
    public float lookClamp;

    public float jumpSpeed;
    public float maxJumpHoldTime;
    Timer jumpTimer;
    bool isOnGround;

    //camera stuff
    private float pitch = 0f;

    Rigidbody rb;
    Camera playerCamera;



    void Start() {

        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();

        jumpTimer = gameObject.AddComponent<Timer>();
        jumpTimer.duration = maxJumpHoldTime;

        Cursor.lockState = CursorLockMode.Locked; //cant move mouse
    }


    void FixedUpdate() {

        Vector2 usin = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        rb.velocity += Vector3.ProjectOnPlane(playerCamera.transform.forward, Vector3.up).normalized * usin.y * moveSpeed * Time.deltaTime;
        rb.velocity += Vector3.ProjectOnPlane(playerCamera.transform.right, Vector3.up).normalized * usin.x * moveSpeed * Time.deltaTime;

        //max movement speed
        float curMaxSpeed = maxSpeed;
        if (Input.GetKey(KeyCode.LeftShift)) {
            curMaxSpeed = sprintMaxSpeed;
        } else if (Input.GetKey(KeyCode.LeftControl)) {
            curMaxSpeed = walkMaxSpeed;
        }

        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x,-curMaxSpeed, curMaxSpeed),rb.velocity.y, Mathf.Clamp(rb.velocity.z, -curMaxSpeed, curMaxSpeed));


        //Jumping
        if (Input.GetKey(KeyCode.Space) && !jumpTimer.isActive && isOnGround) { //if not jumping, start jump
            jumpTimer.activate();
            //possibly add an initial jump impulse force thingy
        }
        if (Input.GetKey(KeyCode.Space) && jumpTimer.isActive) { //hold to jump higher
            rb.velocity += Vector3.up * jumpSpeed * Time.deltaTime;
        } else { //otherwise, if jump key is released, stop jumping
            jumpTimer.deactivate();
        }
        



        isOnGround = false;

    }

    private void Update() {
        //Camera
        var yaw = lookSpeed.x * Input.GetAxis("Mouse X") * Time.deltaTime;
        pitch -= lookSpeed.y * Input.GetAxis("Mouse Y") * Time.deltaTime;

        pitch = Mathf.Clamp(pitch, -lookClamp, lookClamp); //clamp vertical look

        //playerCamera.transform.eulerAngles = new Vector3(pitch, yaw, 0f);
        playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        //rotate in direction of camera
        //NOTE, MAKE IT SO CAMERA IS NOT A CHILD OF THE PLAYER
        transform.Rotate(Vector3.up*yaw);

        //transform.rotation = Quaternion.Euler();
        /*
        Vector3 camRot = playerCamera.transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x, camRot.y, transform.rotation.z), rotateSpeed * Time.deltaTime);
        */



    }

    private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("floor")) {
            isOnGround = true;
        }
    }

}

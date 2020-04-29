using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    public float moveSpeed;
    public float maxSpeed;
    public float moveDamping;
    public float dampThreshold;

    public float rotateSpeed;
    public float accelTiltIntensity;
    public float tiltSpeed;
    Vector3 prevPosition;
    Vector3 prevVelocity;

    public float sprintMaxSpeed;

    public Vector2 lookSpeed;
    public float lookClamp;

    public float jumpSpeed;
    public float maxJumpHoldTime;
    Timer jumpTimer;
    bool isOnGround;

    //camera stuff
    private float pitch = 0f;
    private float yaw = 0f;

    Rigidbody rb;
    Camera playerCamera;

    public GameObject model;

    void Start() {

        prevPosition = transform.position;

        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();

        jumpTimer = gameObject.AddComponent<Timer>();
        jumpTimer.duration = maxJumpHoldTime;

        //Cursor.lockState = CursorLockMode.Locked; //cant move mouse
    }


    void Update() {

        Vector2 usin = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        rb.velocity += Vector3.ProjectOnPlane(playerCamera.transform.forward, Vector3.up).normalized * usin.y * moveSpeed * Time.deltaTime;
        rb.velocity += Vector3.ProjectOnPlane(playerCamera.transform.right, Vector3.up).normalized * usin.x * moveSpeed * Time.deltaTime;

        //apply some friction
        float x_damp = 0;
        if (Mathf.Abs(rb.velocity.x) > dampThreshold) {
            x_damp = -1 * rb.velocity.x / Mathf.Abs(rb.velocity.x) * moveDamping * Time.deltaTime;
        } else {
            rb.velocity = new Vector3(0,rb.velocity.y,rb.velocity.z);
        }
        float z_damp = 0;
        if (Mathf.Abs(rb.velocity.z) > dampThreshold) {
            z_damp = -1 * rb.velocity.z / Mathf.Abs(rb.velocity.z) * moveDamping * Time.deltaTime;
        } else {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
        }
        rb.velocity += new Vector3(x_damp,0,z_damp);


        //max movement speed
        float curMaxSpeed = maxSpeed;
        if (Input.GetKey(KeyCode.LeftShift)) {
            curMaxSpeed = sprintMaxSpeed;
        }

        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x,-curMaxSpeed, curMaxSpeed),rb.velocity.y, Mathf.Clamp(rb.velocity.z, -curMaxSpeed, curMaxSpeed));

        //rotate in direction of movement
        /*
        Vector3 facing = transform.position - prevPosition;
        if (Mathf.Abs(facing.x) > 0 && Mathf.Abs(facing.z) > 0) {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(facing), rotateSpeed * Time.deltaTime);
        }
        */
        

        //tilt in direction of movement
        float accel = Vector3.Magnitude(Vector3.ProjectOnPlane(rb.velocity - prevVelocity, Vector3.up));
        Vector3 tiltAxis = Vector3.Cross(Vector3.ProjectOnPlane(transform.forward,Vector3.up),Vector3.up);
        Quaternion targetTilt = Quaternion.AngleAxis(accel * accelTiltIntensity, tiltAxis);
        model.transform.rotation = Quaternion.Slerp(model.transform.rotation,targetTilt,tiltSpeed*Time.deltaTime);

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
        

        //Camera
        yaw += lookSpeed.x * Input.GetAxis("Mouse X");
        pitch -= lookSpeed.y * Input.GetAxis("Mouse Y");

        pitch = Mathf.Clamp(pitch,-lookClamp,lookClamp); //clamp vertical look

        playerCamera.transform.eulerAngles = new Vector3(pitch, yaw, 0f);

        isOnGround = false;


        //update stuff
        prevPosition = transform.position;
        prevVelocity = rb.velocity;
    }

    private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("floor")) {
            isOnGround = true;
        }
    }

}

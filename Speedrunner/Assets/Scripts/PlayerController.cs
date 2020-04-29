using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    public float moveSpeed;
    public float maxSpeed;


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

        //max movement speed
        float curMaxSpeed = maxSpeed;
        if (Input.GetKey(KeyCode.LeftShift)) {
            curMaxSpeed = sprintMaxSpeed;
        }

        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x,-curMaxSpeed, curMaxSpeed),rb.velocity.y, Mathf.Clamp(rb.velocity.z, -curMaxSpeed, curMaxSpeed));


        //rotate in direction of camera
        Vector3 camRot = playerCamera.transform.rotation.eulerAngles;
        model.transform.rotation = Quaternion.Slerp(model.transform.rotation,Quaternion.Euler(0,camRot.y,0),rotateSpeed*Time.deltaTime);


        //tilt in direction of movement
        Vector3 accel = Vector3.ProjectOnPlane(rb.velocity - prevVelocity,Vector3.up)/Time.deltaTime;

        Vector3 tiltAxis = Vector3.Cross(Vector3.ProjectOnPlane(accel,Vector3.up),Vector3.up);
        Quaternion targetTilt = Quaternion.AngleAxis(Vector3.Magnitude(accel) * accelTiltIntensity, tiltAxis);
        model.transform.localRotation = Quaternion.Slerp(model.transform.localRotation,targetTilt,tiltSpeed*Time.deltaTime);
        

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

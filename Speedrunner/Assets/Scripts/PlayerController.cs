using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    public float moveSpeed;
    public float maxSpeed;
    public float moveDamping;

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

    void Start() {
        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();

        jumpTimer = gameObject.AddComponent<Timer>();
        jumpTimer.duration = maxJumpHoldTime;

        Cursor.lockState = CursorLockMode.Locked; //cant move mouse
    }


    void Update() {

        Vector2 usin = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;


        rb.velocity += Vector3.ProjectOnPlane(playerCamera.transform.forward,Vector3.up).normalized * usin.y * moveSpeed * Time.deltaTime;
        rb.velocity += Vector3.ProjectOnPlane(playerCamera.transform.right, Vector3.up).normalized * usin.x * moveSpeed * Time.deltaTime;

        //apply some friction
        float x_damp = 0;
        if (Mathf.Abs(rb.velocity.x) > 0) {
            x_damp = -1 * rb.velocity.x / Mathf.Abs(rb.velocity.x) * moveDamping * Time.deltaTime;
        }
        float z_damp = 0;
        if (Mathf.Abs(rb.velocity.z) > 0) {
            z_damp = -1 * rb.velocity.z / Mathf.Abs(rb.velocity.z) * moveDamping * Time.deltaTime;
        }
        rb.velocity += new Vector3(x_damp,0,z_damp);


        //max movement speed
        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x,-maxSpeed,maxSpeed),rb.velocity.y, Mathf.Clamp(rb.velocity.z, -maxSpeed, maxSpeed));
        

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

    }

    private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("floor")) {
            isOnGround = true;
        }
    }

}

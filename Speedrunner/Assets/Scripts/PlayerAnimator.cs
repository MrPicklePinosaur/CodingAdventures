using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerAnimator : MonoBehaviour {

    Rigidbody rb;
    Vector3 prevVelocity;

    public GameObject model;

    public float accelTiltIntensity;
    public float tiltSpeed;

    void Start() {
        rb = GetComponent<Rigidbody>();
        prevVelocity = Vector3.zero;
    }


    void Update() {

        //tilt in direction of movement
        Vector3 accel = Vector3.ProjectOnPlane(rb.velocity - prevVelocity, Vector3.up) / Time.deltaTime;

        Vector3 tiltAxis = Vector3.Cross(accel, Vector3.up);
        Quaternion targetTilt = Quaternion.AngleAxis(Vector3.Magnitude(accel) * accelTiltIntensity, tiltAxis);
        model.transform.rotation = Quaternion.Slerp(model.transform.rotation, targetTilt, tiltSpeed * Time.deltaTime);

        //update stuff
        prevVelocity = rb.velocity;

    }
}

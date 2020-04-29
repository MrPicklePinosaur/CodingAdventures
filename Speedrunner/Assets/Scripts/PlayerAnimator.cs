using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerAnimator : MonoBehaviour {

    Rigidbody rb;
    Vector3 prevVelocity;

    public float accelTiltIntensity;
    void Start() {
        rb = GetComponent<Rigidbody>();
        prevVelocity = Vector3.zero;
    }


    void Update() {



        //tilt player based on acceleration
        Vector3 accel = Vector3.ProjectOnPlane(rb.velocity - prevVelocity,Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(accel * accelTiltIntensity),Time.deltaTime*accelTiltIntensity);

        //rb.AddTorque(accel*accelTiltIntensity);

        //apply a restoring force


        //Clamp tilt


        prevVelocity = rb.velocity;

    }
}

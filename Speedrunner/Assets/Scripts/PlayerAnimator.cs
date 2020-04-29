using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerAnimator : MonoBehaviour {

    public float rotateSpeed;
    Rigidbody rb;
    Vector3 prevPosition;
    void Start() {
        rb = GetComponent<Rigidbody>();
        prevPosition = transform.position;
    }


    void Update() {

        Vector3 facing = transform.position - prevPosition;
        //rotate in direction of movement
        if (Mathf.Abs(facing.x) > 0 && Mathf.Abs(facing.z) > 0) {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(facing), rotateSpeed*Time.deltaTime);
        }

        prevPosition = transform.position;

    }
}

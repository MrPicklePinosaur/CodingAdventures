using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {

    public GameObject target;
    public float lookSpeed;
    public float followSpeed;

    public float lookClamp;

    private float pitch = 0f;
    private float yaw = 0f;


    Camera cam;

    void Start() {
        cam = GetComponent<Camera>();

        Cursor.lockState = CursorLockMode.Locked; //cant move mouse
    }

    void Update() {

        //Camera
        yaw += Input.GetAxis("Mouse X");
        pitch -= Input.GetAxis("Mouse Y");

        pitch = Mathf.Clamp(pitch, -lookClamp, lookClamp); //clamp vertical look

        Quaternion targetAngle = Quaternion.Euler(new Vector3(pitch, yaw, 0f));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetAngle, Time.deltaTime*lookSpeed);

        //camera follow
        transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime*followSpeed);


        //adjust FOV based on speed



        //head bob effect
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraEffects : MonoBehaviour {

    public Vector2 lookSpeed;
    public float lookClamp;

    Camera cam;

    void Start() {
        cam = GetComponent<Camera>();
    }

    void Update() {
        
        //look around


        //follow target


        //adjust FOV based on speed



        //head bob effect
    }
}

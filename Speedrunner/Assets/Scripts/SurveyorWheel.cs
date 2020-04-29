﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SurveyorWheel : MonoBehaviour {

    public float runAnimSpeed;
    public float stepDistance;

    public float wheelRadius;

    Animator anim;
    float runStage;
    Vector3 prevPosition;

    void Start() {
        runStage = 0;
        anim = GetComponent<Animator>();

        prevPosition = Vector3.ProjectOnPlane(transform.position,Vector3.up);
    }

    void Update() {

        //turn surveyor wheel
        float dist = Vector3.Distance(Vector3.ProjectOnPlane(transform.position, Vector3.up), prevPosition);
        float turnAngle = (float)(dist / wheelRadius); // theta = s/r
        Debug.Log(turnAngle);

        //update animation
        runStage += turnAngle*runAnimSpeed;

        if (runStage > stepDistance) {
            runStage = 0;
        }

        anim.SetFloat("runStage", runStage/stepDistance);

        if (anim.GetFloat("runStage") > 1f) {
            anim.SetFloat("runStage",0);
        }

        prevPosition = Vector3.ProjectOnPlane(transform.position, Vector3.up);
    }
}

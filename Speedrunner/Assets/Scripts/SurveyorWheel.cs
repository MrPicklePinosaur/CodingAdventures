using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SurveyorWheel : MonoBehaviour {


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

        //find out if we are walking forwards or backwards
        float d = Vector3.Dot(transform.forward, Vector3.ProjectOnPlane(transform.position, Vector3.up)-prevPosition);
        int direct = (d >= 0) ? 1 : -1;

        //turn surveyor wheel
        float dist = direct*Vector3.Distance(Vector3.ProjectOnPlane(transform.position, Vector3.up), prevPosition);
        float turnAngle = (float)(dist / wheelRadius); // theta = s/r

        //update animation
        runStage += turnAngle;

        float wheelDiam = 2 * Mathf.PI * wheelRadius;
        if (runStage > wheelDiam) {
            runStage = 0;
        } else if (runStage < 0) {
            runStage = wheelDiam;
        }

        anim.SetFloat("runStage", runStage/ wheelDiam);

        if (anim.GetFloat("runStage") > 1f) {
            anim.SetFloat("runStage",0);
        }

        prevPosition = Vector3.ProjectOnPlane(transform.position, Vector3.up);
    }
}

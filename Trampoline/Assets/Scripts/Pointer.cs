using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour {

    public float force;
    public float forcePointOffset; //the distance from the surface of the mesh where the force is applied

    void Update() {
        if (Input.GetKey(KeyCode.Mouse0)) {
            FindPoint();
        }
    }

    public void FindPoint() { //raycast onto mesh
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(camRay, out hit)) { //if the ray hit anything

            MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();
            if (deformer) { //if not null
                Vector3 forcepoint = hit.point + hit.normal*forcePointOffset;
                Debug.DrawLine(Camera.main.transform.position,forcepoint);

                
            }

            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshDeformer : MonoBehaviour {

    Vector3[] orgVertices; //position of the original vertices
    Vector3[] dspVertices; //current position of vertices
    Vector3[] velVertices; //velocities of each vertice

    public void InitMeshDeformer() {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        orgVertices = mesh.vertices;
        dspVertices = new Vector3[orgVertices.Length];
        for (int i = 0; i < orgVertices.Length; i++) {
            dspVertices[i] = orgVertices[i];
        }
    }

    public void ApplyForce(float force, Vector3 point) {

    }
}

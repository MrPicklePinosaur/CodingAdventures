using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomMesh))]
public class CustomMeshEditor : Editor {

    public override void OnInspectorGUI() {
        CustomMesh cm = (CustomMesh)target;

        if (DrawDefaultInspector()) { //if a value in inspecter is changed
            cm.GenerateMesh();

        }

        if (GUILayout.Button("Generate Mesh")) {
            cm.GenerateMesh();
        }
    }

}

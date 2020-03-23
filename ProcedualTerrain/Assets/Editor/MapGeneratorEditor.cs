using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor {

    public override void OnInspectorGUI() {
        MapGenerator mg = (MapGenerator)target;

        if (DrawDefaultInspector()) { //if a value in inspecter is changed
            //auto update the texture
            mg.GenerateMap();

        }

        if (GUILayout.Button("Generate Map")) {
            mg.GenerateMap();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewerCam : MonoBehaviour {

    Portal[] portals;

    void Awake() {
        portals = FindObjectsOfType<Portal>();
    }

    void OnPreCull() {
        for (int i = 0; i < portals.Length; i++) {
            portals[i].Render();
        }
    }
}

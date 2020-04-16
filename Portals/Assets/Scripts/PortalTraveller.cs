using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PortalTraveller : MonoBehaviour {

    //public bool ignore_teleport = false;
    GameObject enteredPortal;

    public void Teleport(Transform enter, Transform exit) {
        //ignore_teleport = true; //so the portal we teleport to wont instantly teleport us back
        enteredPortal = enter.gameObject;

        Matrix4x4 mat = exit.localToWorldMatrix * enter.worldToLocalMatrix * transform.localToWorldMatrix;
        transform.SetPositionAndRotation(mat.GetColumn(3), mat.rotation);
    }

    public GameObject getEntrancePortal() {
        return enteredPortal;
    }

    public void resetEntrancePortal() {
        enteredPortal = null;
    }
    
}

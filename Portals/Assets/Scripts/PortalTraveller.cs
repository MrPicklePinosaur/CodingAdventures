using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PortalTraveller : MonoBehaviour {

    public bool ignore_teleport = false;
    public bool attempt_teleport = false;

    Portal enterPortal;
    Vector3 portalForwardVector;

    public void EnterPortal(Portal enter) {

        if (ignore_teleport) { return; }

        enterPortal = enter;
        ignore_teleport = true;
        attempt_teleport = true;

        Vector3 displacement = transform.position - enterPortal.transform.position; //vector from portal to traveller

        //then project the displacement vector onto the portals forward vector  to obtain entrance direction
        portalForwardVector = Vector3.Project(displacement, enterPortal.transform.forward);
    }

    public void AttemptTeleport() { //attempt to teleport
        //Debug.Log("Attempint teleport");
        if (!attempt_teleport) { return; }

        //check to see if traveller is past the middle of the portal
        Vector3 displacement = transform.position - enterPortal.transform.position;
        if (Vector3.Dot(displacement,portalForwardVector) > 0) { return; } //if we are not past the middle of the portal

        GameObject exitPortal = enterPortal.getExitPortal();
        Matrix4x4 mat = exitPortal.transform.localToWorldMatrix * enterPortal.transform.worldToLocalMatrix * transform.localToWorldMatrix;
        transform.SetPositionAndRotation(mat.GetColumn(3), mat.rotation);
        attempt_teleport = false;
    }

    public void ExitPortal(Portal current) {

        if (current.gameObject == enterPortal.getExitPortal() && ignore_teleport) {
            
            ignore_teleport = false;
        }


    }

    private void OnDrawGizmos() {
        if (portalForwardVector != null && enterPortal != null) {
            Gizmos.DrawLine(enterPortal.transform.position,enterPortal.transform.position+enterPortal.transform.forward);
        }
    }



}

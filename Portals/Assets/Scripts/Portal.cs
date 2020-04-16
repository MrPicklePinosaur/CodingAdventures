using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Portal : MonoBehaviour {

    public GameObject linkedPortal;
    Camera portalCam; //used to scout the output
    Camera viewerCam;
    Renderer screen;

    RenderTexture rt;
    BoxCollider col;

    void Awake() {
        portalCam = GetComponentInChildren<Camera>();
        viewerCam = Camera.main;
        screen = GetComponentInChildren<Renderer>();

        portalCam.enabled = false;

        rt = new RenderTexture(Screen.width, Screen.height, 0);
        portalCam.targetTexture = rt;

        screen.material.SetTexture("_MainTex", rt);

        //set collider to same size as portal screen
        col = GetComponent<BoxCollider>();
        col.size = screen.gameObject.transform.localScale;
        col.isTrigger = true;


        //make it so the camera wont clip with the screen
        //AdjustThicknessForClipping();
    }
    
    
    //code from https://github.com/SebLague/Portals/blob/master/Assets/Scripts/Core/Portal.cs
    public void AdjustThicknessForClipping() {
        float halfHeight = viewerCam.nearClipPlane * Mathf.Tan(viewerCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float halfWidth = halfHeight * viewerCam.aspect;
        float dstToNearClipPlaneCorner = new Vector3(halfWidth, halfHeight, viewerCam.nearClipPlane).magnitude;
        float screenThickness = dstToNearClipPlaneCorner;

        Transform screenT = screen.transform;
        bool camFacingSameDirAsPortal = Vector3.Dot(transform.forward, transform.position - viewerCam.transform.position) > 0;
        screenT.localScale = new Vector3(screenT.localScale.x, screenT.localScale.y, screenThickness);
        screenT.localPosition = Vector3.forward * screenThickness * ((camFacingSameDirAsPortal) ? 0.5f : -0.5f);
    }
    
    

    public void Render() {

        AdjustThicknessForClipping();
        updateLinkedCamPosition();

    }


    void updateLinkedCamPosition() {

        //if (screen.isVisibleFrom())

        linkedPortal.GetComponent<Portal>().disableScreen();

        //TODO: actually understand the math 
        Matrix4x4 mat = linkedPortal.transform.localToWorldMatrix * transform.worldToLocalMatrix * viewerCam.transform.localToWorldMatrix;
        portalCam.transform.SetPositionAndRotation(mat.GetColumn(3),mat.rotation);

        portalCam.Render();

        linkedPortal.GetComponent<Portal>().enableScreen();
    }

    public void enableScreen() {
        screen.enabled = true;
    }
    public void disableScreen() {
        screen.enabled = false;
    }


    public GameObject getExitPortal() {
        return linkedPortal;
    }

    private void OnTriggerEnter(Collider other) {
        
        PortalTraveller travel = other.GetComponent<PortalTraveller>();
        if (travel == null) { return; }

        //entering the entrance portal
        travel.EnterPortal(this);
    }

    private void OnTriggerStay(Collider other) {
        PortalTraveller travel = other.GetComponent<PortalTraveller>();
        if (travel == null) { return; }

        travel.AttemptTeleport();
    }


    private void OnTriggerExit(Collider other) {

        PortalTraveller travel = other.GetComponent<PortalTraveller>();
        if (travel == null) { return; }

        travel.ExitPortal(this);
        
    }
    
    
}

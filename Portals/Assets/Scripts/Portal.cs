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
    }

    public void Render() {

        GenerateRenderTexture();
        updateLinkedCamPosition();

    }

    void GenerateRenderTexture() {


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

    private void OnTriggerEnter(Collider other) {
        
        PortalTraveller travel = other.GetComponent<PortalTraveller>();
        if (travel == null) { return; }
        if (travel.getEntrancePortal() != null) { return; }

        travel.Teleport(transform,linkedPortal.transform);


    }

    
    private void OnTriggerExit(Collider other) {
        PortalTraveller travel = other.GetComponent<PortalTraveller>();
        if (travel == null) { return; }

        if (travel.getEntrancePortal() != gameObject) {
            travel.resetEntrancePortal();
        }
    }
    
}

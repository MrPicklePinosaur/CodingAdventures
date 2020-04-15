using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    public GameObject linkedPortal;
    Camera portalCam; //used to scout the output
    Camera viewerCam;
    Renderer screen;

    RenderTexture rt;

    void Start() {
        portalCam = GetComponentInChildren<Camera>();
        viewerCam = Camera.main;
        screen = GetComponentInChildren<Renderer>();

        rt = new RenderTexture(Screen.width, Screen.height,0);
        portalCam.targetTexture = rt;
        portalCam.enabled = false;

        screen.material.SetTexture("_MainTex",rt);

    }

    void Update() {

        updateLinkedCamPosition();

    }

    void updateLinkedCamPosition() {

        linkedPortal.GetComponent<Portal>().disableScreen();

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

}

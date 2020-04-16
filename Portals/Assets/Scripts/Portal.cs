using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    public GameObject linkedPortal;
    Camera portalCam; //used to scout the output
    Camera viewerCam;
    Renderer screen;

    RenderTexture rt;

    void Awake() {
        portalCam = GetComponentInChildren<Camera>();
        viewerCam = Camera.main;
        screen = GetComponentInChildren<Renderer>();

        portalCam.enabled = false;

        rt = new RenderTexture(Screen.width, Screen.height, 0);
        portalCam.targetTexture = rt;

        screen.material.SetTexture("_MainTex", rt);

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

}

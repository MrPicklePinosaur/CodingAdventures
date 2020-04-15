using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Portal : MonoBehaviour {

    public GameObject linkedPortal;
    Camera portalCam; //used to scout the output
    Camera viewerCam;

    RenderTexture rt;

    void Start() {
        portalCam = GetComponentInChildren<Camera>();
        viewerCam = Camera.main;

        rt = new RenderTexture(Screen.width, Screen.height,0);
        portalCam.targetTexture = rt;

        Renderer r = GetComponent<Renderer>();
        r.material.SetTexture("_MainTex",rt);

    }

    void Update() {
        updateLinkedCamPosition();
    }

    void updateLinkedCamPosition() {
        Matrix4x4 mat = linkedPortal.transform.localToWorldMatrix * transform.worldToLocalMatrix * viewerCam.transform.localToWorldMatrix;
        portalCam.transform.SetPositionAndRotation(mat.GetColumn(3),mat.rotation);
    }

    
}

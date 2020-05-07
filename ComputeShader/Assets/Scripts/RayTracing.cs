﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTracing : MonoBehaviour {

    public ComputeShader computeShader;
    RenderTexture target;

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        Render(destination);
    }

    private void Render(RenderTexture destination) {

        CreateRenderTexture();

        //dispatch render texture
        computeShader.SetTexture(0,"Result",target);
        int threadGroupsX = Mathf.CeilToInt(Screen.width / 8f);
        int threadGroupsY = Mathf.CeilToInt(Screen.height / 8f);
        computeShader.Dispatch(0,threadGroupsX,threadGroupsY,1);

        Graphics.Blit(target,destination);
    }

    private void CreateRenderTexture() {
        if (target == null || target.width != Screen.width || target.height != Screen.height) {

            if (target != null) { target.Release(); }

            //create new texture
            target = new RenderTexture(Screen.width,Screen.height,0,RenderTextureFormat.ARGBFloat,RenderTextureReadWrite.Linear);
            target.enableRandomWrite = true;
            target.Create();
        }
    }
}

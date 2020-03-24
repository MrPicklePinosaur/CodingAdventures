using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMesh{

    public Vector3[] verticies;
    public int[] triangles;
    public Vector2[] uvs;

    public CustomMesh(int chunkSize) { //width and height of grid
        verticies = new Vector3[chunkSize * chunkSize];
        triangles = new int[(chunkSize - 1) * (chunkSize - 1) * 6];
        uvs = new Vector2[verticies.Length];
    }

    public Mesh CreateMesh() {
        Mesh mesh = new Mesh();
        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }

    public static CustomMesh GenerateHeightMesh(float[,] noiseMap, float meshHeightModifier, AnimationCurve heightCurve, int lod) {

        int chunkSize = noiseMap.GetLength(0);

        int inc = (lod == 0) ? 1 : lod * 2;
        int vert = (chunkSize - 1) / inc + 1;

        CustomMesh customMesh = new CustomMesh(chunkSize);

        //populate verticies
        for (int v = 0, i = 0; v < chunkSize; v+=inc) {
            for (int u = 0; u < chunkSize; u+=inc, i++) {
                customMesh.verticies[i] = new Vector3(u-(chunkSize - 1)/2f, heightCurve.Evaluate(noiseMap[u,v])*meshHeightModifier, v-(chunkSize - 1)/2f);
                customMesh.uvs[i] = new Vector2((float)u / chunkSize, (float)v / chunkSize);
            }
        }

        //Generate triangles
        //Generate triangles
        for (int v = 0, ti = 0, vi = 0; v < vert-1; v++, vi++) {
            for (int u = 0; u < vert-1; u++, ti += 6, vi++) {
                //triangle 1
                customMesh.triangles[ti] = vi;
                customMesh.triangles[ti + 1] = vi + vert;
                customMesh.triangles[ti + 2] = vi + 1;


                //triangle 2
                customMesh.triangles[ti + 3] = vi + 1;
                customMesh.triangles[ti + 4] = vi + vert;
                customMesh.triangles[ti + 5] = vi + vert + 1;

            }
        }

        return customMesh;

    }

}

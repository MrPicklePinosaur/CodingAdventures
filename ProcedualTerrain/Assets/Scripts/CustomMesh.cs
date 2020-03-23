using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomMesh{


    public static Mesh CreateHeightMesh(float[,] noiseMap) {
        Mesh mesh = new Mesh();
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        Vector3[] verticies = new Vector3[(width + 1) * (height + 1)];
        int[] triangles = new int[width * height * 6];

        //Generate verticies
        for (int i = 0, v = 0; v < height; v++) {
            for (int u = 0; u < width; i++, u++) {
                verticies[i] = new Vector3(u, noiseMap[u,v], v); //horizontal mesh, also the verticies are spaced 1 unit apart
            }
        }

        //Generate triangles
        for (int v = 0; v < height; v++) {
            for (int u = 0, ti = 0; u < width; u++, ti += 6) {
                triangles[ti + 0] = u + v;
                triangles[ti+2] = triangles[ti+4] = (u+1) + v;
                triangles[ti+1] = triangles[ti+5] = (u+width) + v;
                triangles[ti+3] = (u+width+1) + v;
            }
        }

        mesh.vertices = verticies;
        mesh.triangles = triangles;
        return mesh;
    }

}

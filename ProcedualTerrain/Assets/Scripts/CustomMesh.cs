using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMesh{

    public Vector3[] verticies;
    public int[] triangles;
    public Vector2[] uvs;

    public CustomMesh(int width, int height) { //width and height of grid
        verticies = new Vector3[(width + 1) * (height + 1)];
        triangles = new int[width * height * 6];
        uvs = new Vector2[(width + 1) * (height + 1)];
    }

    public Mesh CreateMesh() {
        Mesh mesh = new Mesh();
        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }

    public static CustomMesh GenerateHeightMesh(float[,] noiseMap) {

        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);
        CustomMesh customMesh = new CustomMesh(width, height);

        //Generate verticies
        for (int i = 0, v = 0; v < height; v++) {
            for (int u = 0; u < width; i++, u++) {

                customMesh.verticies[i] = new Vector3(u, noiseMap[u, v], v); //horizontal mesh, also the verticies are spaced 1 unit apart
                customMesh.uvs[i] = new Vector2((float)(u / width), (float)(v / height));
            }
        }

        //Generate triangles
        for (int v = 0; v < height-1; v++) {
            for (int u = 0, ti = 0, vi = 0; u < width-1; u++, ti += 6, vi++) {
                customMesh.triangles[ti+0] = vi;
                customMesh.triangles[ti+2] = customMesh.triangles[ti+4] = vi+1;
                customMesh.triangles[ti+1] = customMesh.triangles[ti+5] = vi+width;
                customMesh.triangles[ti+3] = vi+width+1;
            }
        }

        return customMesh;

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class Meshy : MonoBehaviour {

    public int width;
    public int height;

    Vector3[] verticies;
    int[] triangles;
    Vector2[] uvs;

    private void Start() {
        GenerateMesh();
    }

    void GenerateMesh() {
        verticies = new Vector3[(width+1)*(height+1)];
        triangles = new int[width*height*6];
        uvs = new Vector2[verticies.Length];

        //populate verticies
        for (int v = 0, i = 0; v < height+1; v++) {
            for (int u = 0; u < width+1; u++, i++) {
                verticies[i] = new Vector3(u, 0, v);
                uvs[i] = new Vector2((float)u / width, (float)v / height);
            }
        }

        //Generate triangles
        //Generate triangles
        for (int v = 0, ti = 0, vi = 0; v < height; v++, vi++) {
            for (int u = 0; u < width; u++, ti += 6, vi++) {
                //triangle 1
                triangles[ti] = vi;
                triangles[ti + 1] = vi + width + 1;
                triangles[ti + 2] = vi + 1;

                
                //triangle 2
                triangles[ti + 3] = vi + 1;
                triangles[ti + 4] = vi + width + 1;
                triangles[ti + 5] = vi + width + 2;
                
            }
        }

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos() {
        if (verticies == null) { return; }
        Gizmos.color = Color.blue;

        foreach(Vector3 v in verticies) {
            if (v == null) { continue; }
            Gizmos.DrawSphere(v,0.05f);
        }
    }

}

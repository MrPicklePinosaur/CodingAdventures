using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class CustomMesh : MonoBehaviour {

    public int width;
    public int height;

    Mesh mesh;
    Vector3[] verticies;
    int[] triangles;
    Vector2[] uvs;

    public void GenerateMesh() {

        mesh = new Mesh();
        verticies = new Vector3[(width+1)*(height+1)];
        triangles = new int[width*height*6];
        uvs = new Vector2[verticies.Length];

        for (int v = 0, i = 0; v < height; v++) {
            for (int u = 0; u < width; u++, i++) {
                verticies[i] = new Vector3(u-width/2, 0, v-height/2);
                uvs[i] = new Vector2((float)u/width,(float)v/height);
            }
        }

        for (int v = 0, vi = 0, ti = 0; v < height-1; v++, vi++) {
            for (int u = 0; u < width-1; u++, ti+=6, vi++) {
                //triangle 1
                triangles[ti] = vi;
                triangles[ti+1] = vi + width;
                triangles[ti+2] = vi + 1;

                //triangle 2
                triangles[ti+3] = vi + 1;
                triangles[ti+4] = vi + width;
                triangles[ti+5] = vi + width + 1;
            }
        }

        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().sharedMesh = mesh;

    }

    private void OnDrawGizmos() {
        if (verticies == null) { return; }
        Gizmos.color = Color.blue;
        foreach (Vector3 v in verticies) {
            Gizmos.DrawSphere(v,0.05f);
        }
    }
}

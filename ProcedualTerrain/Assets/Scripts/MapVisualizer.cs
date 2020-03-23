using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapVisualizer : MonoBehaviour {

    public GameObject plane;
    public GameObject mesh;

    public void PlaneVisualizer(Texture2D tex) {
        Renderer r = plane.GetComponent<Renderer>();
        //allows texture to be updated outside of runtime
        r.sharedMaterial.mainTexture = tex;
        r.transform.localScale = new Vector3(tex.width, 1, tex.height);
    }

    public void MeshVisualizer(Texture2D tex, CustomMesh customMesh) {
        MeshFilter meshFilter = mesh.GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = mesh.GetComponent<MeshRenderer>();

        meshFilter.sharedMesh = customMesh.CreateMesh();
        meshRenderer.sharedMaterial.mainTexture = tex;
    }

    public Texture2D GenerateTexture(Color[] colorMap, int width, int height) {
        Texture2D tex = new Texture2D(width, height);
        tex.filterMode = FilterMode.Point;
        tex.wrapMode = TextureWrapMode.Clamp;

        tex.SetPixels(colorMap);
        tex.Apply();

        return tex;
    }

    public Color[] GenerateNoiseColorMap(float[,] noiseMap) {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        Color[] colorMap = new Color[width * height];
        //map noise map onto color map
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        }

        return colorMap;
        
    }
    public Color[] GenerateDepthColorMap(float[,] noiseMap, TerrainLayer[] layers) {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        Color[] colorMap = new Color[width * height];

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                foreach (TerrainLayer t in layers) {
                    if (noiseMap[x, y] <= t.depth) {
                        colorMap[y * width + x] = t.color;
                        break;
                    }
                }
            }
        }
        return colorMap;
    }
        
}

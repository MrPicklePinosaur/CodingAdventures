using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapVisualizer : MonoBehaviour {

    public Renderer plane;


    public void DrawNoiseMap(float[,] noiseMap) {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        Texture2D tex = new Texture2D(width, height);
        tex.filterMode = FilterMode.Point;
        tex.wrapMode = TextureWrapMode.Clamp;

        Color[] colorMap = new Color[width * height];
        //map noise map onto color map
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        }
        tex.SetPixels(colorMap);
        tex.Apply();

        //allows texture to be updated outside of runtime
        plane.sharedMaterial.mainTexture = tex;
        plane.transform.localScale = new Vector3(width, 1, height);
    }

    public void DrawColorMap(float[,] noiseMap, TerrainLayer[] layers) {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        Texture2D tex = new Texture2D(width, height);
        tex.filterMode = FilterMode.Point;
        tex.wrapMode = TextureWrapMode.Clamp;

        Color[] colorMap = new Color[width * height];

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                foreach (TerrainLayer t in layers) {
                    if (noiseMap[x,y] <= t.depth) {
                        colorMap[y * width + x] = t.color;
                        break;
                    }
                }
            }
        }
        tex.SetPixels(colorMap);
        tex.Apply();

        //allows texture to be updated outside of runtime
        plane.sharedMaterial.mainTexture = tex;
        plane.transform.localScale = new Vector3(width, 1, height);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapVisualizer : MonoBehaviour {

    public Renderer plane;

    public void RenderNoiseMap(float[,] noiseMap) {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        Texture2D tex = new Texture2D(width, height);
        Color[] colorMap = new Color[width * height];
        //map noise map onto color map
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                colorMap[y * width + x] = Color.Lerp(Color.white, Color.black, noiseMap[x, y]);
            }
        }
        tex.SetPixels(colorMap);
        tex.Apply();

        //allows texture to be updated outside of runtime
        plane.sharedMaterial.mainTexture = tex;
        plane.transform.localScale = new Vector3(width,1,height);

    }
}

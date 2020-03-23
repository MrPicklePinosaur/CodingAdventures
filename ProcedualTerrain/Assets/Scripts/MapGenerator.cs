using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(MapVisualizer))]
public class MapGenerator : MonoBehaviour {

    public int width;
    public int height;
    public float noiseScale;

    public int octaves; //number of detail iterations
    public float persistance; //amplitude
    public float lacunarity; //frequency

    public void GenerateMap() {
        float[,] noiseMap = GeneratePerlinNoiseMap(width, height, noiseScale,octaves,persistance,lacunarity);

        GetComponent<MapVisualizer>().RenderNoiseMap(noiseMap);
    }

    public static float[,] GeneratePerlinNoiseMap(int width, int height, float scale, int octaves, float persistance, float lacunarity) {
        float[,] noiseMap = new float[width, height];

        Assert.IsTrue(scale > 0, "Invalid noise scale, should be greater than zero");

        float minNoise = float.MaxValue;
        float maxNoise = float.MinValue;

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {

                float amp = 1;
                float freq = 1;
                float noise = 0;

                for (int i = 0; i < octaves; i++) {
                    float perlinValue = Mathf.PerlinNoise(x / scale * freq, y / scale * freq);
                    noise += perlinValue * amp;

                    //modify freq and amp per iteration
                    amp *= persistance;
                    freq *= lacunarity;
                    
                }
                noiseMap[x, y] = noise;

                if (noise < minNoise) { minNoise = noise; }
                if (noise > maxNoise) { maxNoise = noise; }
                
            }
        }

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                noiseMap[x, y] = Mathf.InverseLerp(minNoise, maxNoise, noiseMap[x, y]); //normalize noise map values so it falls between 0 and 1
            }
        }

        return noiseMap;
    }
}

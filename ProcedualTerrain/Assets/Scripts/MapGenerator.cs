using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(MapVisualizer))]
public class MapGenerator : MonoBehaviour {

    public enum DrawMode { NoiseMap, ColorMap, Mesh };
    public DrawMode drawMode;

    public int seed;
    public Vector2 offset;

    public int width;
    public int height;
    public float noiseScale;

    public int octaves; //number of detail iterations
    [Range(0f, 1f)] public float persistance; //amplitude
    public float lacunarity; //frequency

    public TerrainLayer[] layers;

    public void GenerateMap() {
        float[,] noiseMap = GeneratePerlinNoiseMap(seed, offset, width, height, noiseScale, octaves, persistance, lacunarity);

        MapVisualizer mv = GetComponent<MapVisualizer>();
        if (drawMode == DrawMode.NoiseMap) {
            mv.PlaneVisualizer(mv.GenerateNoiseColorMap(noiseMap),width,height);
        } else if (drawMode == DrawMode.ColorMap) {
            mv.PlaneVisualizer(mv.GenerateDepthColorMap(noiseMap,layers),width,height);
        } 
        
        
    }

    public static float[,] GeneratePerlinNoiseMap(int seed, Vector2 offset, int width, int height, float scale, int octaves, float persistance, float lacunarity) {

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves]; //offset each octave
        for (int i = 0; i < octaves; i++) {
            octaveOffsets[i] = new Vector2(prng.Next(-100000, 100000) + offset.x, prng.Next(-100000, 100000) + offset.y);
        }

        float[,] noiseMap = new float[width, height];

        float minNoise = float.MaxValue;
        float maxNoise = float.MinValue;

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {

                float amp = 1;
                float freq = 1;
                float noise = 0;

                for (int i = 0; i < octaves; i++) {
                    float px = (x - width / 2) / scale * freq + octaveOffsets[i].x;
                    float py = (y - height / 2) / scale * freq + octaveOffsets[i].y;
                    float perlinValue = Mathf.PerlinNoise(px, py);
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

    void OnValidate() { //called when one variable is changed
        //clamp all values
        if (width < 1) { width = 1; }
        if (height < 1) { height = 1; }
        if (noiseScale <= 0) { noiseScale = 0.0001f; }
        if (lacunarity < 1) { lacunarity = 1; }
        if (octaves < 0) { octaves = 0; }
    }
}

[System.Serializable]
public struct TerrainLayer {
    public string name;
    [Range(0f, 1f)] public float depth;
    public Color color;
}

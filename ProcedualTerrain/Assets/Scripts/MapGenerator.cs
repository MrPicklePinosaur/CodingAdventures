using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(MapVisualizer))]
public class MapGenerator : MonoBehaviour {

    public int width;
    public int height;
    public float noiseScale;


    public void GenerateMap() {
        float[,] noiseMap = Noise.GeneratePerlinNoiseMap(width, height, noiseScale);

        GetComponent<MapVisualizer>().RenderNoiseMap(noiseMap);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public static class Noise {

    public static float[,] GeneratePerlinNoiseMap(int width, int height, float scale) {
        float[,] noiseMap = new float[width,height];

        Assert.IsTrue(scale > 0, "Invalid noise scale, should be greater than zero");

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                float perlinValue = Mathf.PerlinNoise(x/scale,y/scale);
                noiseMap[x, y] = perlinValue;
            }
        }

        return noiseMap;
    }

    
}

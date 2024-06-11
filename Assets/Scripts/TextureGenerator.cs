using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator
{
    public static Texture2D TextureFromColourMap(Color[] colourMap, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colourMap);
        texture.Apply();
        return texture;
    }

    public static Texture2D TextureFromHeightMap(float[,] heightMap)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        Color[] colourMap = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
            }
        }

        return TextureFromColourMap(colourMap, width, height);
    }

    public static Texture2D FinalNoise(NoiseType noisetype, int mapWidth, int mapHeight, int seed, float NoiseScale, int Octaves, float persistance, float lacunarity, Vector2 offset, Texture2D biomeTex)
    {
        // Generate the noise map
        float[,] noiseMap = Noice.GenerateNoiseMap(noisetype, mapWidth, mapHeight, seed, NoiseScale, Octaves, persistance, lacunarity, offset);

        int texWidth = biomeTex.width;
        int texHeight = biomeTex.height;

        Color[] colourMap = new Color[mapWidth * mapHeight];

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                // Use noise value to determine biome type and get corresponding color from biome texture
                float noiseValue = noiseMap[x, y];

                // Example thresholds for different biomes
                if (noiseValue < 0.3f)
                {
                    // Water biome
                    int xIndex = Mathf.Clamp(Mathf.RoundToInt(noiseValue * texWidth), 0, texWidth - 1);
                    int yIndex = 0;  // Assuming water biome is at the top of the texture
                    colourMap[y * mapWidth + x] = biomeTex.GetPixel(xIndex, yIndex);
                }
                else if (noiseValue < 0.6f)
                {
                    // Plains biome
                    int xIndex = Mathf.Clamp(Mathf.RoundToInt(noiseValue * texWidth), 0, texWidth - 1);
                    int yIndex = Mathf.Clamp(Mathf.RoundToInt(texHeight * 0.5f), 0, texHeight - 1);  // Middle of the texture
                    colourMap[y * mapWidth + x] = biomeTex.GetPixel(xIndex, yIndex);
                }
                else
                {
                    // Mountain biome
                    int xIndex = Mathf.Clamp(Mathf.RoundToInt(noiseValue * texWidth), 0, texWidth - 1);
                    int yIndex = texHeight - 1;  // Assuming mountain biome is at the bottom of the texture
                    colourMap[y * mapWidth + x] = biomeTex.GetPixel(xIndex, yIndex);
                }
            }
        }

        return TextureFromColourMap(colourMap, mapWidth, mapHeight);
    }
}

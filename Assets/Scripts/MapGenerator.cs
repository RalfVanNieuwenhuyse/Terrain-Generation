using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings.SplashScreen;


[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color colour;
}

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColourMap, Mesh, MeshBiome };
    [SerializeField] private DrawMode m_DrawMode;

    [SerializeField] private NoiseType m_NoiseType = NoiseType.Perlin;

    [SerializeField, Min(1)] private int m_MapWidth;
    [SerializeField, Min(1)] private int m_MapHeight;
    [SerializeField, Min(0)] private float m_NoiseScale;

    [SerializeField, Min(1)] private int m_Octaves;
    [SerializeField,Range(0, 1)] private float m_Persistance;
    [SerializeField, Min(1)] private float m_Lacunarity;

    [SerializeField] private int m_Seed;
    [SerializeField] private Vector2 m_Offset;

    [SerializeField] private float m_MeshHeightMultiplier;
    [SerializeField] private AnimationCurve m_MeshHeightCurve;
    [SerializeField] Texture2D m_BiomeTexture;

    [SerializeField] public bool AutoUpdate;

    [SerializeField] private TerrainType[] m_Regions;

    public void GenerateMap()
    {
        float[,] noiseMap = Noice.GenerateNoiseMap(m_NoiseType,m_MapWidth, m_MapHeight, m_Seed, m_NoiseScale, m_Octaves, m_Persistance, m_Lacunarity, m_Offset);

        Color[] colourMap = new Color[m_MapWidth * m_MapHeight];
        for (int y = 0; y < m_MapHeight; y++)
        {
            for (int x = 0; x < m_MapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < m_Regions.Length; i++)
                {
                    if (currentHeight <= m_Regions[i].height)
                    {
                        colourMap[y * m_MapWidth + x] = m_Regions[i].colour;
                        break;
                    }
                }
            }
        }

        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (m_DrawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if (m_DrawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, m_MapWidth, m_MapHeight));
        }
        else if (m_DrawMode == DrawMode.Mesh) {
			display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, m_MeshHeightMultiplier, m_MeshHeightCurve), TextureGenerator.TextureFromColourMap(colourMap, m_MapWidth, m_MapHeight));
		}
        else if(m_DrawMode == DrawMode.MeshBiome)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, m_MeshHeightMultiplier, m_MeshHeightCurve), TextureGenerator.FinalNoise(m_NoiseType, m_MapWidth, m_MapHeight, m_Seed, m_NoiseScale, m_Octaves, m_Persistance, m_Lacunarity, m_Offset, m_BiomeTexture));
        }

    }
}



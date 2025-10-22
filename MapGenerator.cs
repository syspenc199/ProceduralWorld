using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public enum DrawMode {NoiseMap, ColorMap, Mesh};
    public DrawMode drawMode;

    const int mapChunkSize = 241;
    [Range(1,6)]
    public int LevelOfDetail;
    [Min(0)]
    public float ScaleValue;
    public Vector2 Offset;
    [Min(0)]
    public int Octaves;
    public int Seed;
    [Range(0, 1)]
    public float Persistence;
    [Min(1)]
    public float Lacunarity;
    public float HeightMultiplier;
    public AnimationCurve MeshHeightCurve;

    public bool AutoUpdate;

    public TerrainType[] regions;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, Seed, ScaleValue, Offset, Octaves, Persistence, Lacunarity);

        Color[] colorMap = new Color[mapChunkSize * mapChunkSize];
        for (int y = 0; y < mapChunkSize; y++) // Create a color map of the pixels, whether it's a grayscale noise map or a full-blown color map
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colorMap[y * mapChunkSize + x] = regions[i].color; 
                        break;
                    }
                }
            }
        }

        MapDisplay display = FindFirstObjectByType<MapDisplay>();

        if (drawMode == DrawMode.NoiseMap) //Use the draw mode to Determin the type of map
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if(drawMode == DrawMode.ColorMap)
        { 
            display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapChunkSize, mapChunkSize));
        }
        else if(drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, HeightMultiplier, MeshHeightCurve, LevelOfDetail), TextureGenerator.TextureFromColorMap(colorMap, mapChunkSize, mapChunkSize));
        }
    }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}

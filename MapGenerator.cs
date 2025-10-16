using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public enum DrawMode {NoiseMap, ColorMap};
    public DrawMode drawMode;

    [Min(1)]
    public int MapWidth;
    [Min(1)]
    public int MapHeight;
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

    public bool AutoUpdate;

    public TerrainType[] regions;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(MapWidth, MapHeight, Seed, ScaleValue, Offset, Octaves, Persistence, Lacunarity);

        Color[] colorMap = new Color[MapWidth * MapHeight];
        for (int y = 0; y < MapHeight; y++) // Create a color map of the pixels, whether it's a grayscale noise map or a full-blown color map
        {
            for (int x = 0; x < MapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colorMap[y * MapWidth + x] = regions[i].color; 
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
        else { display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, MapWidth, MapHeight));}
    }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}

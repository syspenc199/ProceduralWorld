using UnityEngine;

public static class MeshGenerator
{
    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve, int levelOfDetail)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        float topLeftX = (width-1)/-2f;
        float topLeftZ = (height - 1) / 2f;

        int meshLODIncrement = (levelOfDetail == 0) ? 1 : levelOfDetail * 2;
        int verticiesPerLine = (width - 1) / meshLODIncrement + 1;

        MeshData meshData = new MeshData(verticiesPerLine, verticiesPerLine);
        int vertexIndex = 0;

        for(int y = 0; y < height; y += meshLODIncrement)
        {
            for (int x = 0; x < width; x += meshLODIncrement)
            {
                meshData.Verticies[vertexIndex] = new Vector3(topLeftX + x, heightCurve.Evaluate(heightMap[x,y]) * heightMultiplier, topLeftZ - y);
                meshData.UVs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                if(x < width-1 && y < height-1)
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + verticiesPerLine + 1, vertexIndex + verticiesPerLine); // Add the trianges to every square in the mesh grid
                    meshData.AddTriangle(vertexIndex + verticiesPerLine + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        return meshData;
    }
}

public class MeshData{
    public Vector3[] Verticies;
    public int[] Triangles;
    public Vector2[] UVs;

    int triangleIndex = 0;

    public MeshData(int meshWidth, int meshHeight)
    {
        Verticies = new Vector3[meshWidth * meshHeight];
        UVs = new Vector2[meshWidth * meshHeight];
        Triangles = new int[(meshWidth-1)*(meshHeight-1)*6];
    }

    public void AddTriangle(int a, int b, int c)
    {
        Triangles[triangleIndex] = a;
        Triangles[triangleIndex + 1] = b; // Calculate the coordinates of the trianges
        Triangles[triangleIndex + 2] = c;
        triangleIndex += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh
        {
        vertices = Verticies,
        triangles = Triangles,
        uv = UVs
        };

        return mesh;
    }
}

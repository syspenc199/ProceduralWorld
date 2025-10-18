using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer renderer;
    public MeshFilter meshFilter;
    public MeshRenderer meshRender;

    public void DrawTexture(Texture2D texture)
    {
        renderer.sharedMaterial.mainTexture = texture;
    }

    public void DrawMesh(MeshData meshData, Texture2D texture)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();
        meshRender.sharedMaterial.mainTexture = texture;
    }
}

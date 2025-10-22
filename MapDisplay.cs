using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer meshRenderer;
    public MeshFilter meshFilter;
    public MeshRenderer meshRender;

    public void DrawTexture(Texture2D texture)
    {
        meshRenderer.sharedMaterial.mainTexture = texture;
    }

    public void DrawMesh(MeshData meshData, Texture2D texture)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();
        meshRender.sharedMaterial.mainTexture = texture;
    }
}

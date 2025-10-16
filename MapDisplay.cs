using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer renderer;

    public void DrawTexture(Texture2D texture)
    {
        renderer.sharedMaterial.mainTexture = texture;
    }
}

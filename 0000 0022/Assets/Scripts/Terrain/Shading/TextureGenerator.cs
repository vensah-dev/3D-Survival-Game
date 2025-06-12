using UnityEngine;

public static class TextureGenerator
{
    public static Texture2D ColoruedTexture (Color[] colorMap, int ChunkSize)
    {
        Texture2D texture = new Texture2D (ChunkSize, ChunkSize);
        texture.filterMode = FilterMode.Trilinear;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colorMap);
        texture.Apply();

        return texture;
    }
}
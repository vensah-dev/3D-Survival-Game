using UnityEngine;

public class Grass : MonoBehaviour
{
    public Texture2D grassTexture; // The texture representing the grass detail

    public void PaintGrass(Terrain terrain)
    {
        int detailMapWidth = terrain.terrainData.detailWidth;
        int detailMapHeight = terrain.terrainData.detailHeight;

        // Get the detail layer for the specified index
        int[,] detailLayer = new int[detailMapHeight, detailMapWidth];

        // Calculate the size of the grass texture in relation to the detail map
        int grassTextureWidth = detailMapWidth / grassTexture.width;
        int grassTextureHeight = detailMapHeight / grassTexture.height;

        // Loop through the detail map and paint grass where needed
        for (int y = 0; y < detailMapHeight; y += grassTextureHeight)
        {
            for (int x = 0; x < detailMapWidth; x += grassTextureWidth)
            {
                // Calculate the average color of the grass texture within the current block
                Color avgColor = CalculateAverageColor(x, y, grassTextureWidth, grassTextureHeight);

                // If the average color is bright (indicating grass), paint the detail
                if (avgColor.grayscale > 0.5f)
                {
                    detailLayer[y, x] = 1; // Set the detail value to indicate presence of grass
                }
            }
        }

        int detailLayerIndex = terrain.terrainData.detailPrototypes.Length; // Create a new detail layer
        terrain.terrainData.SetDetailResolution(detailMapWidth, detailMapHeight);
        terrain.terrainData.detailPrototypes = new DetailPrototype[] { new DetailPrototype { prototypeTexture = grassTexture } };
        // Apply the modified detail layer back to the terrain
        terrain.terrainData.SetDetailLayer(0, 0, detailLayerIndex, detailLayer);
    }

    Color CalculateAverageColor(int startX, int startY, int width, int height)
    {
        Color avgColor = Color.black;

        for (int y = startY; y < startY + height; y++)
        {
            for (int x = startX; x < startX + width; x++)
            {
                avgColor += grassTexture.GetPixel(x % grassTexture.width, y % grassTexture.height);
            }
        }

        avgColor /= (width * height);

        return avgColor;
    }
}

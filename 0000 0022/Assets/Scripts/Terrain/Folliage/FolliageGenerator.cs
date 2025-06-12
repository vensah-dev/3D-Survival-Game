using System.IO.Ports;
using UnityEngine;

public class FolliageGenerator : MonoBehaviour
{

    TerrrainEditor terrrainEditor;

    public Folliage[] folliageList;

    void SpawnTree(Vector3 spawnPosition, Terrain terrain, Folliage follaige)
    {

        float minHeight = follaige.MinHeight;
        float maxHeight = follaige.MaxHeight;

        Vector3 minSize = follaige.MinSize;
        Vector3 maxSize = follaige.MaxSize;

        float offset = follaige.Offset;


        Vector3 RotV = new Vector3(0, Random.Range(0, 360), 0);
        Quaternion Rotation = Quaternion.Euler(RotV);

        float spawnPositiony = terrain.SampleHeight(spawnPosition);

        spawnPosition = new Vector3(spawnPosition.x + Random.Range(-offset, offset), spawnPositiony, spawnPosition.z + Random.Range(-offset, offset));

        if (minHeight <= spawnPositiony && maxHeight >= spawnPositiony)
        {
            GameObject Clone;

            Clone = Instantiate(follaige.Prefab, spawnPosition, Rotation);
            Clone.transform.parent = transform;

            Vector3 size;

            size.x = Random.Range(minSize.x, maxSize.x);
            size.y = Random.Range(minSize.y, maxSize.y);
            size.z = Random.Range(minSize.z, maxSize.z);

            Clone.transform.localScale = size;

        }

    }



    public void Generate(Terrain terrain)
    {

        terrrainEditor = FindObjectOfType<TerrrainEditor>();

        int noiseMapRes = terrrainEditor.TerrainSize * terrrainEditor.TerrainResolution + 1;

        for (int i = 0; i < folliageList.Length; i++)
        {
            Folliage f = folliageList[i];

            int seed = 0;
            float[,] noiseMap = noise.GenerateNoise(noiseMapRes, seed, f.NoiseScale, f.Octaves, f.Persistance, f.Lacunarity, new Vector2(0, 0), noise.NormalizeMode.Local);

            FolliageData.NoiseMaps.Insert(f.id, noiseMap);

        }


        for (int i = 0; i < folliageList.Length; i++)
        {

            float[,] folliageNoiseMap = FolliageData.NoiseMaps[folliageList[i].id];

            for (int x = 0; x < noiseMapRes; x++)
            {
                for (int y = 0; y < noiseMapRes; y++)
                {

                    Vector3 position = new Vector3(((x - (noiseMapRes / 2)) * (terrrainEditor.Scale)), 0, ((y - (noiseMapRes / 2)) * (terrrainEditor.Scale)));

                    if (folliageNoiseMap[x, y] < folliageList[i].Density)
                    {
                        SpawnTree(position, terrain, folliageList[i]);
                    }

                    if (folliageList[i].randomSpawns)
                    {
                        if (Random.Range(0, folliageList[i].Rarity) == folliageList[i].Rarity - 1)
                        {
                            SpawnTree(position, terrain, folliageList[i]);

                        }

                    }
                }
                //end of generator code in for loops
            }
        }



    }

        //end of generator code loops
}
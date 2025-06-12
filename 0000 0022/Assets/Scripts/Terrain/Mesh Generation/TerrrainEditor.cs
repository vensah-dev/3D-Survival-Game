using UnityEngine; 
using System.Collections.Generic;
using System;
using System.Threading;

public class TerrrainEditor : MonoBehaviour
{
    Mesh mesh;
    MeshCollider meshCollider;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    [Space]
    [Header("Terrain Settings")]

    public Grass grass;
    public FolliageGenerator treeGenerator;

    public Material TerrainMaterial;

    public int DetailResolution;
    public int ResolutionPerPatch;

    public int Scale = 10;

    public int TerrainSize = 100;
    public int TerrainResolution = 10;

    public noise.NormalizeMode normalizeMode;

    [Space]
    [Header("Map Settings")]
    [Range(0, 6)]

    public float heightMultiplier;

    public AnimationCurve HeightCurve;

    public bool ApplyAnimationCurve;


    [Header("Falloff Map")]
    public bool FalloffMap;

    [Range(0.001f, 5f)]
    public float IslandShapeA = 3f;

    [Range(0.001f, 10f)]
    public float FalloffSizeB = 2.2f;

    [Header("Seeds and Noise Data")]
    public string Seed = "Original Flavour Seed";
    public int ActualSeed = 0;
    public int TreeSeed = 0;
    public int RockSeed = 0;

    public float[,] noiseMap;
    public float[,] TreeNoise;
    public float[,] RockNoise;

    public Vector2 Centre;
    public Vector2 offset;

    public bool Generate;

    [Space]
    [Header("Perlin Noise Settings")]
    public float lacunarity = 2f;

    [Range(0, 3)]
    public float persistance = 0.5f;

    public float NoiseScale = 24.2f;

    public int Octaves = 3;


    [Space]
    [Header("Tree Noise Settings")]
    public float TreeLacunarity = 2f;

    [Range(0, 3)]
    public float TreePersistance = 0.5f;

    public float TreeNoiseScale = 24.2f;

    public int TreeOctaves = 3;


    [Space]
    [Header("Rock Noise Settings")]
    public float RockLacunarity = 2f;

    [Range(0, 3)]
    public float RockPersistance = 0.5f;

    public float RockNoiseScale = 24.2f;

    public int RockOctaves = 3;

    private float maxLocalTerrainHeight = 0f;
    private float minLocalTerrainHeight = 0f;

    public GameObject terrain;

    private void Awake()
    {
        ActualSeed = Seed.GetHashCode();
        UnityEngine.Random.InitState(ActualSeed);
    }


    void Start()
    {
        Generate = false;

        GenerateTerrain();

    }

    void SetFolliageSeeds()
    {

        TreeSeed = Mathf.RoundToInt(ActualSeed * ActualSeed - 15 / ActualSeed - (ActualSeed / 3));
        RockSeed = Mathf.RoundToInt(ActualSeed * ActualSeed - 16 / ActualSeed - (ActualSeed / 4));

    }

    void GenerateRandomSeed()
    {
        int TempoSeed = UnityEngine.Random.Range(-999999, 999999);

        Seed = TempoSeed.ToString();

        SetFolliageSeeds();

    }

    void GenerateTerrainData(Vector2 centre)
    {
        ActualSeed = Seed.GetHashCode();
        SetFolliageSeeds();

        int noiseMapRes = TerrainSize * TerrainResolution + 1;

        noiseMap = noise.GenerateNoise(noiseMapRes, ActualSeed, NoiseScale, Octaves, persistance, lacunarity, centre + offset, normalizeMode);


        Centre = centre;
        float[,] falloffMap = Falloff.GenerateFalloffMap(noiseMapRes, IslandShapeA, FalloffSizeB);

        Color[] ColorMap = new Color[noiseMapRes * noiseMapRes];



        for (int y = 0; y < noiseMapRes; y++)
        {
            for (int x = 0; x < noiseMapRes; x++)
            {
                if (FalloffMap)
                {
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);
                }

                float Height;

                float noiseHeight = noiseMap[x, y];

                if (noiseHeight > maxLocalTerrainHeight)
                {
                    maxLocalTerrainHeight = noiseHeight;
                }
                else if (noiseHeight < minLocalTerrainHeight)
                {
                    minLocalTerrainHeight = noiseHeight;
                }

                if (ApplyAnimationCurve)
                {
                    Height = HeightCurve.Evaluate(noiseHeight);
                }
                else
                {
                    Height = noiseHeight * heightMultiplier;
                }

                noiseMap[x, y] = Height;

            }
        }
    }

    void GenerateTerrain()
    {
        GenerateTerrainData(new Vector2(0, 0));

        TerrainData terrainData = new TerrainData();


        terrainData.SetDetailResolution(DetailResolution, ResolutionPerPatch);
        terrainData.heightmapResolution = TerrainSize * TerrainResolution;
        terrainData.size = new Vector3(TerrainSize * TerrainResolution * Scale, heightMultiplier * Scale, TerrainSize * TerrainResolution * Scale);

        terrainData.SetHeights(0, 0, noiseMap);

        GameObject terrain = Terrain.CreateTerrainGameObject(terrainData);

        terrain.transform.localScale = new Vector3(Scale, Scale, Scale);

        Vector3 size = terrain.GetComponent<Terrain>().terrainData.size;
        Vector3 offset = new Vector3(size.x * 0.5f, 0f, size.z * 0.5f);
        Vector3 currentPos = terrain.transform.position;
        terrain.transform.position = currentPos - offset;

        terrain.transform.SetParent(this.transform);
        terrain.layer = LayerMask.NameToLayer("Walkable");
        terrain.GetComponent<Terrain>().materialTemplate = TerrainMaterial;

        treeGenerator.Generate(terrain.GetComponent<Terrain>());
        grass.PaintGrass(terrain.GetComponent<Terrain>());
    }
}

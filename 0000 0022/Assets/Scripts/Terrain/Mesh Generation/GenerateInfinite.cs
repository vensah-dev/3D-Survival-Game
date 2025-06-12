//using System.Collections.Generic;
//using UnityEngine;

//public class GenerateInfinite : MonoBehaviour
//{
//    const float PlayerMovementThreshholdForChunkUpdate = 25f;
//    const float SquarePlayerMovementThreshholdForChunkUpdate = PlayerMovementThreshholdForChunkUpdate * PlayerMovementThreshholdForChunkUpdate;
//    const float ScaleOfChunks = 15f;

//    public GameObject Trees;

//    public LODInfo[] detailLevels; 
//    public static float maxViewDist;
//    public Transform player;
//    public Material mapMaterial;

//    public static Vector2 playerPos;
//    Vector2 playerPosOld;
//    int chunkSize;
//    int chunksVisible;

//    public static TerrrainEditor terrainEditor;
//    public static GenerateInfinite generateInfinite;



//    Dictionary<Vector2, TerrainChunk> TChunckDictionary = new Dictionary<Vector2, TerrainChunk>();
//    static List<TerrainChunk> chunksVisiblePreviousUpdate = new List<TerrainChunk>();

//    void Start()
//    {
//        terrainEditor = FindObjectOfType<TerrrainEditor>();
//        generateInfinite = FindObjectOfType<GenerateInfinite>();

//        maxViewDist = detailLevels[detailLevels.Length - 1].LODVisibleThreshold;
//        chunkSize = TerrrainEditor.notChunkSize - 1;
//        chunksVisible = Mathf.RoundToInt(maxViewDist / chunkSize);

//        updateVisibleChunks();


//    }

//    void Update()
//    {
//        playerPos = new Vector2(player.position.x, player.position.z) / ScaleOfChunks;

//        if ((playerPosOld - playerPos).sqrMagnitude > SquarePlayerMovementThreshholdForChunkUpdate)
//        {
//            playerPosOld = playerPos;

//            updateVisibleChunks();
//        }
//    }

//    void updateVisibleChunks()
//    {

//        for (int i = 0; i < chunksVisiblePreviousUpdate.Count; i++)
//        {
//            chunksVisiblePreviousUpdate[i].setVisibility(false);
//        }

//        chunksVisiblePreviousUpdate.Clear();

//        int currentChunkCoordX = Mathf.RoundToInt(playerPos.x / chunkSize);
//        int currentChunkCoordY = Mathf.RoundToInt(playerPos.y / chunkSize);

//        for (int yOffset = -chunksVisible; yOffset <= chunksVisible; yOffset++)
//        {
//            for (int xOffset = -chunksVisible; xOffset <= chunksVisible; xOffset++)
//            {
//                Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

//                if (TChunckDictionary.ContainsKey(viewedChunkCoord))
//                {
//                    TChunckDictionary[viewedChunkCoord].updateChunk();
//                }
//                else
//                {
//                    TChunckDictionary.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, chunkSize, detailLevels, transform, mapMaterial));
//                }

//            }
//        }
//    }

//    public class TerrainChunk : MonoBehaviour
//    {

//        public static MeshData meshdataT;
//        public GameObject Chunks;
//        Vector2 Pos;
//        Bounds bounds;

//        MeshRenderer meshRenderer;
//        MeshFilter meshFilter;
//        MeshCollider meshCollider;

//        LODInfo[] detailLevels;
//        LODMesh[] LODMeshes;

//        MapData mapData;
//        bool mapDataReceived;
//        int previousLODIndex = -1;
//        float[,] TreeMap;

//        private void Start()
//        {
//        }

//        public TerrainChunk(Vector2 coord, int size, LODInfo[] detailLevels, Transform Parent, Material material)
//        {

//            this.detailLevels = detailLevels;

//            Pos = coord * size;
//            bounds = new Bounds(Pos, Vector2.one * size);
//            Vector3 Pos3D = new Vector3(Pos.x, 0, Pos.y) * ScaleOfChunks;
//            int ChunkSize = 241;

//            Chunks = new GameObject("Chunk");
//            meshRenderer = Chunks.AddComponent<MeshRenderer>();
//            meshFilter = Chunks.AddComponent<MeshFilter>();
//            meshCollider = Chunks.AddComponent<MeshCollider>();
//            meshRenderer.material = material;
//            meshdataT = FindObjectOfType<MeshData>();

//            Chunks.layer = LayerMask.NameToLayer("GND");
//            Chunks.transform.position = Pos3D;
//            Chunks.transform.localScale = Vector3.one * ScaleOfChunks;
//            Parent.transform.parent = Parent;
//            setVisibility(false);

//            TreeMap = noise.GenerateNoise(241, terrainEditor.TreeSeed, 100f, 3, 0.5f, 2f, new Vector2(0, 0), terrainEditor.normalizeMode);

//            LODMeshes = new LODMesh[detailLevels.Length];
//            for (int i = 0; i < detailLevels.Length; i++)
//            {
//                LODMeshes[i] = new LODMesh(detailLevels[i].LOD, updateChunk);

//            }


//            for (int x = 0; x < ChunkSize; x++)
//            {
//                for (int y = 0; y < ChunkSize; y++)
//                {
//                    if (TreeMap[x, y] == 1.2f)
//                    {
//                        Debug.Log(TreeMap[x, y]);
//                        Vector3 TreePos = new Vector3(Pos3D.x + (x * ScaleOfChunks), 113, Pos3D.z + (y * ScaleOfChunks));
//                        Vector3 RotationV = new Vector3(0, 0, 0);
//                        Quaternion TreeRotation = Quaternion.Euler(RotationV);

//                        GameObject Current = Instantiate(generateInfinite.Trees, TreePos, TreeRotation);

//                        Current.transform.parent = Chunks.transform;
//                    }
//                }
//            }

//            terrainEditor.requestMapData(Pos, onMapDataReceived);
//        }

//        void onMapDataReceived(MapData mapData)
//        {
//            this.mapData = mapData;
//            mapDataReceived = true;

//            Texture2D texture = TextureGenerator.ColoruedTexture(mapData.ColorMap, TerrrainEditor.notChunkSize);
//            meshRenderer.material.mainTexture = texture;

//            updateChunk();
//        }

//        public void updateChunk()
//        {
//            if (mapDataReceived)
//            {
//                //***** also change this to multiplay rather than Mathf.sprt *****\\
//                float playerDisFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(playerPos));

//                bool CVisibility = playerDisFromNearestEdge <= maxViewDist;

//                if (CVisibility)
//                {
//                    int lodIndex = 0;

//                    for (int i = 0; i < detailLevels.Length - 1; i++)
//                    {
//                        if (playerDisFromNearestEdge > detailLevels[i].LODVisibleThreshold)
//                        {
//                            lodIndex = i + 1;

//                        }
//                        else
//                        {
//                            break;
//                        }
//                    }

//                    if (lodIndex != previousLODIndex)
//                    {
//                        LODMesh lODMesh = LODMeshes[lodIndex];
//                        if (lODMesh.hasMesh)
//                        {
//                            previousLODIndex = lodIndex;
//                            meshFilter.mesh = lODMesh.mesh;
//                            meshCollider.sharedMesh = lODMesh.mesh;
//                        }
//                        else if (!lODMesh.hasMesh)
//                        {
//                            lODMesh.requestMesh(mapData);

//                        }
//                    }

//                    chunksVisiblePreviousUpdate.Add(this);
//                }

//                setVisibility(CVisibility);
//            }
           
//        }

//        public void setVisibility(bool visibility)
//        {
//            Chunks.SetActive(visibility);
//        }

//        public bool currentVisibility()
//        {
//            return Chunks.activeSelf;
//        }
//    }

//    class LODMesh
//    {
//        public Mesh mesh;
//        public bool hasRequestedMesh;
//        public bool hasMesh;
//        int LOD;
//        System.Action updateCallback;

//        public LODMesh(int LOD, System.Action updateCallback)
//        {
//            this.LOD = LOD;
//            this.updateCallback = updateCallback;


//        }

//        void onMeshDataReceived(MeshData meshData)
//        {
//            mesh = meshData.AssembleMesh();
//            hasMesh = true;

//            updateCallback();
//        }

//        public void requestMesh(MapData mapData)
//        {
//            hasRequestedMesh = true;
//            terrainEditor.requestMeshData(mapData, LOD, onMeshDataReceived);
//        }


//    }

//    [System.Serializable]
//    public struct LODInfo
//    {
//        public int LOD;
//        public float LODVisibleThreshold;


//    }

//}
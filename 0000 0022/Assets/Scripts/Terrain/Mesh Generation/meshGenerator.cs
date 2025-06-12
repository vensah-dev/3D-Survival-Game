using UnityEngine;

public static class meshGenerator
{
    public static MeshData GenerateTerrainMesh(float[,] noiseMap, float heightMultiplier, int LOD, AnimationCurve _heightCurve, bool ApplyAnimationCurve)
    {
        AnimationCurve heightCurve = new AnimationCurve(_heightCurve.keys);

        int Increment = (LOD == 0) ? 1 : LOD * 2;

        int borderSize = noiseMap.GetLength(0);
        int meshSize = borderSize - 2 * Increment;
        int meshSizeUnsimplified = borderSize - 2;

        float topLeftX = (meshSizeUnsimplified - 1) / -2f;
        float topLeftY = (meshSizeUnsimplified - 1) / 2f;

        if (Increment == 0)
        {
            Increment = 1;
        }

        int VertPerLine = (meshSize - 1) / Increment + 1;


        MeshData meshData = new MeshData(VertPerLine);

        int[,] vertIMap = new int[borderSize, borderSize];
        int meshVertexI = 0;
        int borderVertexI = -1;

        for (int y = 0; y < borderSize; y += Increment)
        {
            for (int x = 0; x < borderSize; x += Increment)
            {
                bool isBorderVer = y == 0 || y == borderSize - 1 || x == 0 || x == borderSize - 1;

                if (isBorderVer)
                {
                    vertIMap[x, y] = borderVertexI;
                    borderVertexI--;
                }
                else
                {
                    vertIMap[x, y] = meshVertexI;
                    meshVertexI++;
                }
            }
        }

        for (int y = 0; y < borderSize; y += Increment)
        {
            for (int x = 0; x < borderSize; x += Increment)
            {
                int vertI = vertIMap[x, y];

                Vector2 percent = new Vector2((x - Increment) / (float)meshSize, (y - Increment) / (float)meshSize);
                float Height;

                if (ApplyAnimationCurve)
                {
                    Height = heightCurve.Evaluate(noiseMap[x, y]) * heightMultiplier;
                }
                else
                {
                    Height = noiseMap[x, y] * heightMultiplier;
                }

                Vector3 VertexPos = new Vector3(topLeftX + percent.x * meshSizeUnsimplified, Height, topLeftY - percent.y * meshSizeUnsimplified);

                meshData.addVert(VertexPos, percent, vertI);

                if (x < borderSize - 1 && y < borderSize - 1)
                {
                    int a = vertIMap[x, y];
                    int b = vertIMap[x + Increment, y];
                    int c = vertIMap[x, y + Increment];
                    int d = vertIMap[x + Increment, y + Increment];

                    meshData.AddTri(a, d, c);
                    meshData.AddTri(d, a, b);
                }

                vertI++;
            }
        }


        return meshData;
    }
}


public class MeshData : MonoBehaviour
{


    public Vector3[] verticies;
    int[] triangles;
    Vector2[] uvs;


    Vector3[] borderVerts;
    int[] borderTs;

    int triI;
    int BtriI;

    public MeshData(int vertPerLine)
    {
        verticies = new Vector3[vertPerLine * vertPerLine];
        uvs = new Vector2[vertPerLine * vertPerLine];
        triangles = new int[(vertPerLine - 1) * (vertPerLine - 1) * 6];

        borderVerts = new Vector3[(vertPerLine * 4) + 4];
        borderTs = new int[24 * vertPerLine];
    }

    public void addVert(Vector3 vertPos, Vector2 vertUVs, int vertI)
    {
        if (vertI < 0)
        {
            borderVerts[-vertI - 1] = vertPos;
        }
        else
        {
            verticies[vertI] = vertPos;
            uvs[vertI] = new Vector2(0, vertPos.y);
        }
    }

    public void AddTri(int a, int b, int c)
    {

        if (a < 0 || b < 0 || c < 0)
        {
            borderTs[BtriI] = a;
            borderTs[BtriI + 1] = b;
            borderTs[BtriI + 2] = c;

            BtriI += 3;
        }
        else
        {
            triangles[triI] = a;
            triangles[triI + 1] = b;
            triangles[triI + 2] = c;

            triI += 3;
        }
    }


    Vector3[] ClaculateNormals()
    {
        Vector3[] vertexNormals = new Vector3[verticies.Length];

        int TriCount = triangles.Length / 3;
        int BTriCount = borderTs.Length / 3;

        for (int i = 0; i < TriCount; i++)
        {
            int normalTriIndex = i * 3;
            int vertexIndexA = triangles[normalTriIndex];
            int vertexIndexB = triangles[normalTriIndex + 1];
            int vertexIndexC = triangles[normalTriIndex + 2];

            Vector3 triangleNormal = calculateNormalsOfIndecies(vertexIndexA, vertexIndexB, vertexIndexC);

            vertexNormals[vertexIndexA] += triangleNormal;
            vertexNormals[vertexIndexB] += triangleNormal;
            vertexNormals[vertexIndexC] += triangleNormal;
        }

        for (int i = 0; i < BTriCount; i++)
        {
            int normalTriIndex = i * 3;
            int vertexIndexA = borderTs[normalTriIndex];
            int vertexIndexB = borderTs[normalTriIndex + 1];
            int vertexIndexC = borderTs[normalTriIndex + 2];

            Vector3 triangleNormal = calculateNormalsOfIndecies(vertexIndexA, vertexIndexB, vertexIndexC);

            if (vertexIndexA >= 0)
            {
                vertexNormals[vertexIndexA] += triangleNormal;
            }
            if (vertexIndexB >= 0)
            {
                vertexNormals[vertexIndexB] += triangleNormal;
            }
            if (vertexIndexC >= 0)
            {
                vertexNormals[vertexIndexC] += triangleNormal;
            }
        }

        for (int i = 0; i < vertexNormals.Length; i++)
        {
            vertexNormals[i].Normalize();
        }

        return vertexNormals;
    }

    Vector3 calculateNormalsOfIndecies(int iA, int iB, int iC)
    {
        Vector3 pA = (iA < 0) ? borderVerts[-iA - 1] : verticies[iA];
        Vector3 pB = (iB < 0) ? borderVerts[-iB - 1] : verticies[iB];
        Vector3 pC = (iC < 0) ? borderVerts[-iC - 1] : verticies[iC];

        Vector3 sAB = pB - pA;
        Vector3 sAC = pC - pA;

        return Vector3.Cross(sAB, sAC);


    }

    public Mesh AssembleMesh()
    {
        Mesh mesh = new Mesh();

        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.normals = ClaculateNormals();

        return mesh;
    }
} 
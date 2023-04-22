using UnityEngine;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    public Material terrainMaterial;

    [Range(-10000f, 10000f)]
    public float xOffsetSeed = 0f;
    [Range(-10000f, 10000f)]
    public float yOffsetSeed = 0f;
    [Range(0, 1.0f)]
    public float waterProbabiltyPerCell = .4f;
    public float scale = .1f;
    public int size = 100;

    public bool enableIslandStructure = false;
    Cell[,] grid;

    void Awake()
    {
        GenerateTerrianMap();
        DrawTerrainMesh(grid);
        DrawTexture(grid);
    }

    void GenerateTerrianMap()
    {

        float[,] noiseMap = new float[size, size];
        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * scale + xOffsetSeed, z * scale + yOffsetSeed);
                noiseMap[x, z] = noiseValue;
            }
        }

        grid = new Cell[size, size];
        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = noiseMap[x, z];

                if (enableIslandStructure) noiseValue -= calculateFallOffMapValue(x, z, size);

                bool isWater = noiseValue < waterProbabiltyPerCell;

                Cell cell = grid[x, z];

                if (cell == null) cell = new Cell(isWater);
                else cell.isWater = isWater;

                grid[x, z] = cell;
            }
        }



    }

    private float calculateFallOffMapValue(int x, int y, int size)
    {
        float xv = x / (float)size * 2 - 1;
        float yv = y / (float)size * 2 - 1;
        float v = Mathf.Max(Mathf.Abs(xv), Mathf.Abs(yv));
        return Mathf.Pow(v, 3f) / (Mathf.Pow(v, 3f) + Mathf.Pow(2.2f - 2.2f * v, 3f));
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            // generate a terrian map for edit mode
            GenerateTerrianMap();

            // set up start drawing position so that the Transform Gizmo can be displayed.
            // in the center of the map in edit mode.
            float startX = this.transform.position.x - size / 2;
            float startZ = this.transform.position.z - size / 2;

            float startY = this.transform.position.y;

            for (int z = 0; z < size; z++)
            {
                for (int x = 0; x < size; x++)
                {
                    Cell cell = grid[x, z];
                    if (cell.isWater)
                        Gizmos.color = Color.blue;
                    else
                        Gizmos.color = Color.green;
                    Vector3 pos = new Vector3(x + startX, startY, z + startZ);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }

        }
    }

    void DrawTerrainMesh(Cell[,] grid)
    {
        float startX = this.transform.position.x - size / 2;
        float startZ = this.transform.position.z - size / 2;

        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, z];
                // if (!cell.isWater)
                // {
                Vector3 a = new Vector3(startX + x - .5f, 0, startZ + z + .5f);
                Vector3 b = new Vector3(startX + x + .5f, 0, startZ + z + .5f);
                Vector3 c = new Vector3(startX + x - .5f, 0, startZ + z - .5f);
                Vector3 d = new Vector3(startX + x + .5f, 0, startZ + z - .5f);
                Vector2 uvA = new Vector2(x / (float)size, z / (float)size);
                Vector2 uvB = new Vector2((x + 1) / (float)size, z / (float)size);
                Vector2 uvC = new Vector2(x / (float)size, (z + 1) / (float)size);
                Vector2 uvD = new Vector2((x + 1) / (float)size, (z + 1) / (float)size);
                Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                Vector2[] uv = new Vector2[] { uvA, uvB, uvC, uvB, uvD, uvC };
                for (int k = 0; k < 6; k++)
                {
                    vertices.Add(v[k]);
                    triangles.Add(triangles.Count);
                    uvs.Add(uv[k]);
                }
                // }
            }
        }
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
    }

    void DrawTexture(Cell[,] grid)
    {
        Texture2D texture = new Texture2D(size, size);
        Color[] colorMap = new Color[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                if (cell.isWater)
                    colorMap[y * size + x] = Color.blue;
                else
                    colorMap[y * size + x] = Color.green;
            }
        }
        texture.filterMode = FilterMode.Point;
        texture.SetPixels(colorMap);
        texture.Apply();

        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshRenderer.material = terrainMaterial;
        meshRenderer.material.mainTexture = texture;
    }
}
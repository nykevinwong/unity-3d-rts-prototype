using UnityEngine;
using System.Collections.Generic;


public class Grid : MonoBehaviour
{
    public Material terrainMaterial;
    public Material edgeMaterial;

    [Range(-10000f, 10000f)]
    public float xOffsetSeed = 0f;
    [Range(-10000f, 10000f)]
    public float yOffsetSeed = 0f;
    [Range(0, 1.0f)]
    public float waterProbabiltyPerCell = .4f;
    public float scale = .1f;
    public int size = 100;
    public Color groundColor;


    public bool enableIslandStructure = false;
    public bool centerTerrian = false;
    public float treeNoiseScale = .05f;
    public float treeDensity = .5f;

    public Cell[,] grid;


    public GameObject[] treePrefabs;

    void Awake()
    {
        setupBoxColliderPosition();
        GenerateTerrianMap();
        DrawTerrainMesh(grid);
        DrawEdgeMesh(grid);
        DrawTexture(grid);
        GenerateTrees(grid);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
                    setupBoxColliderPosition();
                    GenerateTerrianMap();
                    DrawTerrainMesh(grid);
                    DrawEdgeMesh(grid);
                    DrawTexture(grid);
        }
    }

    void setupBoxColliderPosition()
    {
        BoxCollider bc = this.GetComponent<BoxCollider>();
        if(centerTerrian==false) 
        {
           bc.center = new Vector3(bc.center.x  +size/2, 0 , bc.center.z +size/2);
        }
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
            float startX = getStartX();
            float startZ = getStartZ();

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

    float getStartX()
    {
        if(centerTerrian)
        {
            return this.transform.position.x - size / 2;;
        }

        return this.transform.position.x;
    }

    float getStartZ()
    {
        if(centerTerrian)
        {
            return this.transform.position.z - size / 2;;
        }

        return this.transform.position.z;
    }
    

    void DrawTerrainMesh(Cell[,] grid)
    {
        float startX = getStartX();
        float startZ = getStartZ();

        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, z];
                if (!cell.isWater)
                {
                    // Vector3 a = new Vector3(startX + x - .5f, 0, startZ + z + .5f);
                    // Vector3 b = new Vector3(startX + x + .5f, 0, startZ + z + .5f);
                    // Vector3 c = new Vector3(startX + x - .5f, 0, startZ + z - .5f);
                    // Vector3 d = new Vector3(startX + x + .5f, 0, startZ + z - .5f);
                    Vector3 a = new Vector3(startX + x , 0, startZ + z );
                    Vector3 b = new Vector3(startX + x + 1f, 0, startZ + z );
                    Vector3 c = new Vector3(startX + x , 0, startZ + z - 1f);
                    Vector3 d = new Vector3(startX + x + 1f, 0, startZ + z - 1f);
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
                }
                else
                {
                    // Vector3 a = new Vector3(startX + x - .5f, -.5f, startZ + z + .5f);
                    // Vector3 b = new Vector3(startX + x + .5f, -.5f, startZ + z + .5f);
                    // Vector3 c = new Vector3(startX + x - .5f, -.5f, startZ + z - .5f);
                    // Vector3 d = new Vector3(startX + x + .5f, -.5f, startZ + z - .5f);
                    // Vector2 uvA = new Vector2(x / (float)size, z / (float)size);
                    // Vector2 uvB = new Vector2((x + 1) / (float)size, z / (float)size);
                    // Vector2 uvC = new Vector2(x / (float)size, (z + 1) / (float)size);
                    // Vector2 uvD = new Vector2((x + 1) / (float)size, (z + 1) / (float)size);
                    // Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                    // Vector2[] uv = new Vector2[] { uvA, uvB, uvC, uvB, uvD, uvC };
                    // for (int k = 0; k < 6; k++)
                    // {
                    //     vertices.Add(v[k]);
                    //     triangles.Add(triangles.Count);
                    //     uvs.Add(uv[k]);
                    // }

                }
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

    void addQuad(List<Vector3> vertices, List<int> triangles,
                     Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        Vector3[] v = new Vector3[] { a, b, c, b, d, c };
        for (int k = 0; k < 6; k++)
        {
            vertices.Add(v[k]);
            triangles.Add(triangles.Count);
        }
    }

    void DrawEdgeMesh(Cell[,] grid)
    {
        float startX = getStartX();
        float startZ = getStartZ();

        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                if (!cell.isWater)
                {
                    if (x > 0)
                    {
                        Cell left = grid[x - 1, y];
                        if (left.isWater)
                        {
                            Vector3 a = new Vector3(startX + x , 0, startZ + y );
                            Vector3 b = new Vector3(startX + x , 0, startZ + y - 1f);
                            Vector3 c = new Vector3(startX + x , -1, startZ + y );
                            Vector3 d = new Vector3(startX + x , -1, startZ + y - 1f);
                            Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                            for (int k = 0; k < 6; k++)
                            {
                                vertices.Add(v[k]);
                                triangles.Add(triangles.Count);
                            }
                        }
                    }
                    if (x < size - 1)
                    {
                        Cell right = grid[x + 1, y];
                        if (right.isWater)
                        {
                            Vector3 a = new Vector3(startX + x + 1f, 0, startZ + y + 1f);
                            Vector3 b = new Vector3(startX + x + 1f, 0, startZ + y );
                            Vector3 c = new Vector3(startX + x + 1f, -1, startZ + y + 1f);
                            Vector3 d = new Vector3(startX + x + 1f, -1, startZ + y );
                            Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                            for (int k = 0; k < 6; k++)
                            {
                                vertices.Add(v[k]);
                                triangles.Add(triangles.Count);
                            }
                        }
                    }
                    if (y > 0)
                    {
                        Cell down = grid[x, y - 1];
                        if (down.isWater)
                        {
                            Vector3 a = new Vector3(startX + x , 0, startZ + y - 1f);
                            Vector3 b = new Vector3(startX + x + 1f, 0, startZ + y - 1f);
                            Vector3 c = new Vector3(startX + x , -1, startZ + y - 1f);
                            Vector3 d = new Vector3(startX + x + 1f, -1, startZ + y - 1f);
                            Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                            for (int k = 0; k < 6; k++)
                            {
                                vertices.Add(v[k]);
                                triangles.Add(triangles.Count);
                            }
                        }
                    }
                    if (y < size - 1)
                    {
                        Cell up = grid[x, y + 1];
                        if (up.isWater)
                        {
                            Vector3 a = new Vector3(startX + x + 1f, 0, startZ + y );
                            Vector3 b = new Vector3(startX + x , 0, startZ + y );
                            Vector3 c = new Vector3(startX + x + 1f, -1, startZ + y );
                            Vector3 d = new Vector3(startX + x , -1, startZ + y );
                            Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                            for (int k = 0; k < 6; k++)
                            {
                                vertices.Add(v[k]);
                                triangles.Add(triangles.Count);
                            }
                        }
                    }
                }
            }
        }
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        GameObject edgeObj = new GameObject("Edge");
        edgeObj.transform.SetParent(transform);

        MeshFilter meshFilter = edgeObj.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = edgeObj.AddComponent<MeshRenderer>();
        meshRenderer.material = edgeMaterial;
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
                    colorMap[y * size + x] = Color.white;
                else
                    colorMap[y * size + x] = groundColor;
            }
        }
        texture.filterMode = FilterMode.Point;
        texture.SetPixels(colorMap);
        texture.Apply();

        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshRenderer.material = terrainMaterial;
        meshRenderer.material.mainTexture = texture;
    }

    void GenerateTrees(Cell[,] grid) {
        float[,] noiseMap = new float[size, size];
        (float xOffset, float yOffset) = (Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        for(int y = 0; y < size; y++) {
            for(int x = 0; x < size; x++) {
                float noiseValue = Mathf.PerlinNoise(x * treeNoiseScale + xOffset, y * treeNoiseScale + yOffset);
                noiseMap[x, y] = noiseValue;
            }
        }


        float startX = getStartX();
        float startZ = getStartZ();

        for(int y = 0; y < size; y++) {
            for(int x = 0; x < size; x++) {
                Cell cell = grid[x, y];
                if(!cell.isWater) {
                    float v = Random.Range(0f, treeDensity);
                    if(noiseMap[x, y] < v) {
                        GameObject prefab = treePrefabs[Random.Range(0, treePrefabs.Length)];
                        GameObject tree = Instantiate(prefab, transform);
                        tree.transform.position = new Vector3(startX+x+.5f, 0, startZ+y+.5f);
                        tree.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
                        tree.transform.localScale = Vector3.one * Random.Range(.2f, .4f);
                    }
                }
            }
        }
    }

}
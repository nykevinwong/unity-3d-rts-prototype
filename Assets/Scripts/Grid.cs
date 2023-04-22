using UnityEngine;

public class Grid : MonoBehaviour
{
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
        }

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
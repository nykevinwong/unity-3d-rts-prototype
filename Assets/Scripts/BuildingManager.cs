using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://x-team.com/blog/unity-3d-best-practices-physics/
public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }

    Camera mainCamera;
    public BuildingTypeSO buildingType;
    public Transform cursor;
    public int size = 100;
    public bool[,] occupied;
    bool keyDownT = false;

    private GameObject grid;
    private Grid gridScript;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        mainCamera = Camera.main;        
        occupied = new bool[size,size];

        grid = GameObject.Find("Grid");
        gridScript = grid.GetComponent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
            Transform buildingPrefab = buildingType.prefab;
            int tileW = buildingType.width, tileH = buildingType.length;
            float halfTileW = tileW/2f, halfTileH = tileH/2f;
            RaycastHit hit;
            bool isRayHit = Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit);

            int x = (int)(hit.point.x);
            int z = (int)(hit.point.z);

            if(isRayHit)
            {
                if(hit.transform.tag == "TerrianMap")
                {        
                    DebugUtils.DrawRect(new Vector3(x,0,z), new Vector3(x+tileW,0,z+tileH), Color.white);
                }
            }

            //TODO: currently, press T Key won't always place the building.
            if(Input.GetKeyDown(KeyCode.T) && isRayHit)
            {
                if(hit.transform.tag == "TerrianMap")
                {                    
                    Debug.Log("x:"+x +", z:"+ z);
                    if(IsOccupiedByBuilding(x,z, tileW, tileH) || 
                    !isPlacibleLandScape(x,z+1, tileW, tileH))   // only z+1 works well for now.                 
                    {
                        DebugUtils.DrawRect(new Vector3(x,0,z), new Vector3(x+tileW,0,z+tileH), Color.red, 3f);
                        Debug.DrawLine(new Vector3(x,0,z), new Vector3(x+tileW,0,z+tileH), Color.red, 3f);
                        Debug.DrawLine(new Vector3(x+tileW,0,z), new Vector3(x,0,z+tileH), Color.red, 3f);
                    }
                    else
                    {

                        // to do check whether the area is placible?
                        // (1) is the area all flat?
                        // (2) is there any obstacle such as water, stone, mountain or tree?

                            DebugUtils.DrawRect(new Vector3(x,0,z), new Vector3(x+tileW,0,z+tileH), Color.magenta, 3f);
                        
                            Vector3 pos = new Vector3(x+halfTileW, 0, z+halfTileH);
                            Instantiate(buildingPrefab, pos, Quaternion.identity * buildingPrefab.localRotation);
                            Occupy(x,z, tileW, tileH);
                    }


                
                }
            }

    }

    public void SetActiveBuildingType(BuildingTypeSO buildingType) 
    {
        this.buildingType = buildingType;
    }

    public bool IsOccupiedByBuilding(int x, int z, int tileW, int tileH)
    {
        for(int i=0;i < tileH; i++)
            for(int j=0; j < tileW;j++)
            {
                if(occupied[x + j, z + i]) return true;
            }

        return false;
    }

    public void Occupy(int x, int z)
    {
        if(x < 0 || z < 0 || x >= size || z >= size) return;
        occupied[x, z] = true;
    }

    public void Occupy(int x, int z, int tileW, int tileH)
    {
        for(int i=0;i < tileH; i++)
            for(int j=0; j < tileW;j++)
            {
                occupied[x + j, z + i] = true;
            }
    }

    public void Empty(int x, int z)
    {
        occupied[x, z] = false;
    }

    public bool isPlacibleLandScape(int x, int z, int tileW, int tileH)
    {
        for(int i=0;i < tileH; i++)
            for(int j=0; j < tileW;j++)
            {
                Cell c = gridScript.grid[x + j, z + i];
                if(c.isWater) return false;
                if(c.hasTree) return false;
            }

        return true;
    }



}

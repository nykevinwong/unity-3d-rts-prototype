using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://x-team.com/blog/unity-3d-best-practices-physics/
public class BuildingManager : MonoBehaviour
{
    Camera mainCamera;
    public Transform buildingPrefab;
    public Transform cursor;
    public int size = 100;
    bool[,] occupied;
    bool keyDownT = false;

    private GameObject grid;
    private Grid gridScript;

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
            int tileW = 2, tileH = 2;
            float halfTileW = tileW/2, halfTileH = tileH/2;
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

                            Vector3 pos = new Vector3(x+halfTileW, 0, z+halfTileH);
                            Instantiate(buildingPrefab, pos, Quaternion.identity * buildingPrefab.localRotation);
                            Occupy(x,z, tileW, tileH);
                    }


                
                }
            }

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

    public void Occupy(int x, int z, int tileW, int tileH)
    {
        for(int i=0;i < tileH; i++)
            for(int j=0; j < tileW;j++)
            {
                occupied[x + j, z + i] = true;
            }
    }

    public bool isPlacibleLandScape(int x, int z, int tileW, int tileH)
    {
        for(int i=0;i < tileH; i++)
            for(int j=0; j < tileW;j++)
            {
                Cell c = gridScript.grid[x + j, z + i];
                if(c.isWater) return false;
            }

        return true;
    }



}

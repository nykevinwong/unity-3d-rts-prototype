using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// https://x-team.com/blog/unity-3d-best-practices-physics/
public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }

    public class DetectPlacibleSpotEventArgs : EventArgs
    {
        public bool IsPlacible { get; private set; }
        public Vector3 Pos { get; private set; }
        public BuildingTypeSO BuildingType;

        public DetectPlacibleSpotEventArgs(bool isPlacible, Vector3 pos, BuildingTypeSO buildingType)
        {
            this.IsPlacible = isPlacible;
            this.Pos = pos;
            this.BuildingType = buildingType;
        }
    };

    public class PlacibleSpotSelectedEventArgs : EventArgs
    {
        public Vector3 Pos { get; private set; }
        public BuildingTypeSO BuildingType;

        public PlacibleSpotSelectedEventArgs(Vector3 pos, BuildingTypeSO buildingType)
        {
            this.Pos = pos;
            this.BuildingType = buildingType;
        }
    };

    public event EventHandler<DetectPlacibleSpotEventArgs> detectPlacibleSpotEventHandler;
    public event EventHandler<PlacibleSpotSelectedEventArgs> placibleSpotSelectedEventHandler;
    
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
        occupied = new bool[size,size];
        Instance = this;
    }

    void Start()
    {
        BuildingManager.Instance.placibleSpotSelectedEventHandler += OnPlacibleSpotSelected;

        mainCamera = Camera.main;        

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

            bool isLeftMouseButtonDown = Input.GetMouseButtonDown(0);
            
            if(isRayHit)
            {
                if(hit.transform.tag == "TerrianMap")
                {        

                    if(IsOccupiedByBuilding(x,z, tileW, tileH) || 
                    !isPlacibleLandScape(x,z+1, tileW, tileH))   // only z+1 works well for now.                 
                    {                        
                        detectPlacibleSpotEventHandler?.Invoke(this,new DetectPlacibleSpotEventArgs(false, new Vector3(x,0,z), buildingType));

                        DebugUtils.DrawRect(new Vector3(x,0,z), new Vector3(x+tileW,0,z+tileH), Color.red);
                        Debug.DrawLine(new Vector3(x,0,z), new Vector3(x+tileW,0,z+tileH), Color.red);
                        Debug.DrawLine(new Vector3(x+tileW,0,z), new Vector3(x,0,z+tileH), Color.red);
                    }
                    else
                    {

                       if(isLeftMouseButtonDown)
                       {
                        Debug.Log("Building Manager onPlacibleSpotSelected event");
                        placibleSpotSelectedEventHandler?.Invoke(this,new PlacibleSpotSelectedEventArgs(new Vector3(x,0,z), buildingType));                       
                       }
                       else
                       {
                        
                        detectPlacibleSpotEventHandler?.Invoke(this,new DetectPlacibleSpotEventArgs(true, new Vector3(x,0,z), buildingType));
                        DebugUtils.DrawRect(new Vector3(x,0,z), new Vector3(x+tileW,0,z+tileH), Color.white);

                       }
                    }

                }
            }

    }

    public void OnPlacibleSpotSelected(object sender, PlacibleSpotSelectedEventArgs args)
    {
        Transform buildingPrefab = buildingType.prefab;
        
        int tileW = buildingType.width, tileL = buildingType.length;
        float halfTileW = tileW/2f, halfTileL= tileL/2f;
        
        Vector3 sourcePos = args.Pos;        
        Vector3 targetPos = new Vector3(sourcePos.x+halfTileW, 0, sourcePos.z+halfTileL);

        Instantiate(buildingPrefab, targetPos, Quaternion.identity * buildingPrefab.localRotation);
        
        Occupy((int)sourcePos.x,(int)sourcePos.z, tileW, tileL);

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

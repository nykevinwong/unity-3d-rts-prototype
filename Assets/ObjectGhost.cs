using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGhost : MonoBehaviour
{
    public static ObjectGhost Instance { get; private set; }
    // Start is called before the first frame update
    private Transform building;
    private Renderer renderer;

        public enum SurfaceType
    {
        Opaque,
        Transparent
    }

public enum BlendMode
{
    Alpha,
    Premultiply,
    Additive,
    Multiply
}

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        BuildUI.Instance.buildTypeSelectedEventHanlder += BuildUI_OnBuildTypeSelected;
        BuildingManager.Instance.placibleSpotSelectedEventHandler += BuildingManager_OnPlacibleSpotSelected;
        BuildingManager.Instance.detectPlacibleSpotEventHandler += BuildingManager_OnDetectPlacibleSpot;
    }

    // Update is called once per frame
    void Update()
    {
    }
    void BuildingManager_OnDetectPlacibleSpot(object sender, BuildingManager.DetectPlacibleSpotEventArgs args)
    {
        int tileW = args.BuildingType.width, tileL = args.BuildingType.length;
        float halfTileW = tileW/2f, halfTileL= tileL/2f;
        
        Vector3 sourcePos = args.Pos;        
        Vector3 targetPos = new Vector3(sourcePos.x+halfTileW, 0, sourcePos.z+halfTileL);
        building.transform.position = targetPos;

        Color targetColor = args.IsPlacible ? new Color(0.0f, 1.0f, 0.0f, 1.0f) : 
        new Color(1.0f, 0.0f, 0.0f, 1.0f);

         // !!!!! Set Surface Type to Transparent as I understand
      //  renderer.material.SetString("_SurfaceType", 0.5f);
        renderer.material.SetFloat("_Surface", (float)SurfaceType.Transparent);
        renderer.material.SetFloat("_Blend", (float)BlendMode.Additive);
        // !!!!! Set Alpha chanel to 0.3
        renderer.material.SetColor("_BaseColor", targetColor);
       // Call SetColor using the shader property name "_Color" and setting the color to red
      //  renderer.material.SetColor("_Color", Color.red);

    }

    void BuildingManager_OnPlacibleSpotSelected(object sender, BuildingManager.PlacibleSpotSelectedEventArgs args)
    {
        if(building!=null) Destroy(building.gameObject);
        building=null;
    }

    void BuildUI_OnBuildTypeSelected(object sender, BuildUI.BuildTypeSelectedEventArgs args)
    {        
        if(building!=null) Destroy(building.gameObject);

        building = Instantiate(args.BuildingType.prefab, this.transform.position, Quaternion.identity * args.BuildingType.prefab.localRotation); 
        building.GetComponent<ResourceGenerator>().enabled = false;
//        building.transform.localRotation = Quaternion.Euler(-90, 180, 0); 
        building.transform.parent = gameObject.transform;             
        renderer = building.GetComponent<Renderer>();
    }



}

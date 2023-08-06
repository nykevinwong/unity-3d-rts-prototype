using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionUI : MonoBehaviour
{
    public static SelectionUI Instance { get; private set; }
    public Dictionary<int, GameObject> selectedUnitList = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> SelectedUnits { get { return selectedUnitList; }}

    Camera mainCamera;
    Vector3 start, end;
    bool isDragSelect;

    public float moveDetla = 40;

    void BuildUI_OnBuildTypeSelected(object sender, BuildUI.BuildTypeSelectedEventArgs args)
    {
        SelectionUI.Instance.DeselectAll();
    }


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        BuildUI.Instance.buildTypeSelectedEventHanlder += BuildUI_OnBuildTypeSelected;
        mainCamera = Camera.main;
    }

    void Update()
    {
        // check whether player is currently using build placement feature 
        // ToDO: decouple BuildingManager dependency if possible. use event delegate or global event system?
        if(BuildingManager.Instance.buildingType!=null) return;

        if(Input.GetMouseButtonDown(0))
        {
            start = Input.mousePosition;
        }

        if(Input.GetMouseButton(0))// while left button is held
        {
            float curMoveDelta = (start-Input.mousePosition).magnitude;           
            isDragSelect = (curMoveDelta > moveDetla);
        }
        
        if(Input.GetMouseButtonUp(0))// while left button is held
        {
            if(isDragSelect==false)
            {
                Ray ray = mainCamera.ScreenPointToRay(start);
                RaycastHit hit;
                bool isRayHit = Physics.Raycast(ray, out hit, 50000.0f);
                bool isLeftShiftDown = Input.GetKey(KeyCode.LeftControl);

                if(isRayHit)
                {
                    if(hit.transform.tag == "TerrianMap")
                    {
                        SelectionUI.Instance.DeselectAll();
                        // move units?
                    }
                    else if(isLeftShiftDown) // inclusive select/multi-select another unit 
                    {
                        SelectionUI.Instance.Select(hit.transform.gameObject);
                    }
                    else // single select
                    {
                        SelectionUI.Instance.DeselectAll();
                        SelectionUI.Instance.Select(hit.transform.gameObject);
                    }

                }
                else // if we didn't hit anything. deselect
                {
                    if(isLeftShiftDown) 
                    {
                        // do nothing or deselect within this area
                    }
                    else
                    {
                        SelectionUI.Instance.DeselectAll();
                    }


                }

            }
            
        }

    }

    public void Select(GameObject go)
    {
        int id = go.GetInstanceID();
        SelectedIndicator selectedIndicator = go.GetComponent<SelectedIndicator>();

        if(!selectedUnitList.ContainsKey(id)) 
        {
            if(selectedIndicator==null) 
            {
                selectedIndicator = go.AddComponent<SelectedIndicator>();
            }

            selectedIndicator.Selected();

            Debug.Log($"SelectionUI: select {id}");
            selectedUnitList.Add(id, go);
        }
        else
        {
            Debug.Log($"SelectionUI:  selecgt {id} -> already contains {id}");
        }

        
    }

    public void Deselect(int id, Dictionary<int, GameObject> targetSelectedTable)
    {
        if(targetSelectedTable.ContainsKey(id)) 
        {
            if(targetSelectedTable[id]!=null)
            {                
                SelectedIndicator selectedIndicator = targetSelectedTable[id].GetComponent<SelectedIndicator>();                
                selectedIndicator.Unselected();
                Debug.Log($"SelectionUI: deselect {id}");
            }
            else
            {
                Debug.Log($"SelectionUI: unable to delselect {id}, which contains null value.");
            }

            targetSelectedTable.Remove(id);
        }
        else
        {
             Debug.Log($"SelectionUI: deselect {id} does not exist");
        }
    }

    public void Deselect(GameObject go, Dictionary<int, GameObject> targetSelectedTable)
    {
        int id = go.GetInstanceID();
        Deselect(id, targetSelectedTable);
    }

    public void DeselectAll()
    {
        Dictionary<int, GameObject> tempTable = new Dictionary<int, GameObject>(selectedUnitList);

        foreach(KeyValuePair<int, GameObject> pair in selectedUnitList)
        {
            Deselect(pair.Key, tempTable);
        }

        tempTable.Clear();
        selectedUnitList.Clear(); // clear null-value objects   
    }

}

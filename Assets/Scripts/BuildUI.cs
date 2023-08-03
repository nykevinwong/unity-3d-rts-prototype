using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BuildUI : MonoBehaviour
{
    public static BuildUI Instance { get; private set;}
    BuildingTypeListSO buildingTypeListSO;    
    public float offsetY = 0;
    public float offsetX = 105;
    // Start is called before the first frame update

    public class BuildTypeSelectedEventArgs : EventArgs
    {
        public BuildingTypeSO BuildingType { get; private set; }
        public BuildTypeSelectedEventArgs(BuildingTypeSO buildingType)
        {
            this.BuildingType = buildingType;
        }
    };

    public event EventHandler<BuildTypeSelectedEventArgs> buildTypeSelectedEventHanlder;

    void Awake()
    {
        buildingTypeListSO = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);

        Transform buildTypeTemplate = transform.Find("btnBuildTypeTemplate");
        buildTypeTemplate.gameObject.SetActive(false);

        Vector2 initPos = buildTypeTemplate.GetComponent<RectTransform>().anchoredPosition;
        int index = 0;


        foreach (BuildingTypeSO buildingType in buildingTypeListSO.list)
        {
            Transform buildTypeTransform = Instantiate(buildTypeTemplate, transform);

            buildTypeTransform.gameObject.SetActive(true);
            buildTypeTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(initPos.x + offsetX * index, initPos.y + offsetY * index);

            buildTypeTransform.Find("buttonText").GetComponent<TextMeshProUGUI>().SetText(buildingType.nameString);
           // Image image = buildTypeTransform.Find("imageBackground").GetComponent<Image>();


            buildTypeTransform.GetComponent<Button>().onClick.AddListener( () => {                
                buildTypeSelectedEventHanlder(this, new BuildTypeSelectedEventArgs(buildingType));
            });

            index++;

            Debug.Log(buildingType.nameString + " added to resourceUI");

        }

        Instance = this;
    }

    void Start()
    {
       // buildTypeSelectedEventHanlder(this, new BuildTypeSelectedEventArgs(buildingTypeListSO.list[0]));
    }


    // Update is called once per frame
    void Update()
    {

    }
}

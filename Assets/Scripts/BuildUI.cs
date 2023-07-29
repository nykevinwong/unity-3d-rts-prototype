using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BuildUI : MonoBehaviour
{
    public float offsetY = 0;
    public float offsetX = 105;
    // Start is called before the first frame update
        BuildingTypeListSO buildingTypeListSO;
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
                BuildingManager.Instance.SetActiveBuildingType(buildingType);
            });



            index++;

            Debug.Log(buildingType.nameString + " added to resourceUI");

        }
    }

    void Start()
    {
        BuildingManager.Instance.SetActiveBuildingType(buildingTypeListSO.list[0]);
    }


    // Update is called once per frame
    void Update()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    private float timer, timerMax;
    private BuildingTypeSO buildingType;
    // Start is called before the first frame update
    void Start()
    {
        buildingType = this.GetComponent<BuildingTypeHolder>().buildingType;
        timerMax = buildingType.resourceGeneratorData.timerMax;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= timerMax)
        {
            timer -= timerMax; // reset
            ResourceManager.Instance.AddResourceAmount(buildingType.resourceGeneratorData.resourceType,1);
        }
    }
}

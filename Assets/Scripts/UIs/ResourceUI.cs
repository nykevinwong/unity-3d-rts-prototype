using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResourceUI : MonoBehaviour
{
    public float offsetY = 50;
    public float offsetX = 0;
    // Start is called before the first frame update

    private Dictionary<ResourceTypeSO, TextMeshProUGUI> resourceTextFields = new Dictionary<ResourceTypeSO, TextMeshProUGUI>();
    void Awake()
    {
        ResourceTypeListSO resourceTypeListSO = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

        Transform resourceTemplate = transform.Find("resourceTemplate");
        resourceTemplate.gameObject.SetActive(false);
        Vector2 initPos = resourceTemplate.GetComponent<RectTransform>().anchoredPosition;
        int index = 0;
        foreach (ResourceTypeSO resourceType in resourceTypeListSO.list)
        {
            Transform resourceTransform = Instantiate(resourceTemplate, transform);

            resourceTransform.gameObject.SetActive(true);
            resourceTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(initPos.x - offsetX * index, initPos.y - offsetY * index);
            // Debug.Log("x;" + (initPos.x - offsetX * index) + ", y: " + (initPos.y - offsetY * index));
            resourceTransform.Find("image").GetComponent<Image>().sprite = resourceType.sprite;

            // int resourceAmount = ResourceManager.Instance.GetResourceAmount(resourceType);
            resourceTextFields[resourceType] = resourceTransform.Find("text").GetComponent<TextMeshProUGUI>();
            index++;

            Debug.Log(resourceType.nameString + " added to resourceUI");

        }
    }

    void Start()
    {
        ResourceManager.Instance.onResourceAmountChanged += ResourceAmountChanged;
    }

    private void ResourceAmountChanged(object sender, ResourceManager.ResourceUpdateEventArg arg)
    {
        resourceTextFields[arg.type].SetText(arg.value.ToString());
    }


    // Update is called once per frame
    void Update()
    {

    }
}

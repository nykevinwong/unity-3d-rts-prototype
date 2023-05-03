using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResourceManager : MonoBehaviour
{
    public class ResourceUpdateEventArg : EventArgs
    {
        public ResourceUpdateEventArg(ResourceTypeSO type, int resourceAmount)
        {
            this.type = type;
            this.value = resourceAmount;
        }
        public ResourceTypeSO type;
        public int value;
    };


    public static ResourceManager Instance { get; private set; }
    private ResourceTypeListSO resourceTypeListSO;
    private Dictionary<ResourceTypeSO, int> resources = new Dictionary<ResourceTypeSO, int>();
    public event EventHandler<ResourceUpdateEventArg> onResourceAmountChanged;
    void Awake()
    {
        Instance = this;
        resourceTypeListSO = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

        Debug.Log("loaded " + resourceTypeListSO.list.Count);

        foreach (ResourceTypeSO rt in resourceTypeListSO.list)
        {
            resources.Add(rt, rt.value);
        }

    }


    void Update()
    {

        foreach (ResourceTypeSO rt in resourceTypeListSO.list)
        {
            //  Debug.Log(rt.name + " = " + resources[rt]);
        }
    }

    public void AddResourceAmount(ResourceTypeSO type, int value)
    {
        resources[type] += value;
        Debug.Log(type.nameString + " = " + resources[type]);

        onResourceAmountChanged?.Invoke(this, new ResourceUpdateEventArg(type, resources[type]));
    }

    public int GetResourceAmount(ResourceTypeSO type)
    {
        return resources[type];
    }

}
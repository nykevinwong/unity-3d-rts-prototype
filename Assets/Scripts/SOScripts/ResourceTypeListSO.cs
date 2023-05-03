using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Resource Type List")]
public class ResourceTypeListSO : ScriptableObject
{
    public List<ResourceTypeSO> list;
}

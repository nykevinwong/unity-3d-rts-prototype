using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Building Type")]
public class BuildingTypeSO : ScriptableObject
{
    public string nameString;
    public Transform prefab;
    public int width = 2;
    public int length = 2;
    public ResourceGeneratorData resourceGeneratorData = new ResourceGeneratorData();
}

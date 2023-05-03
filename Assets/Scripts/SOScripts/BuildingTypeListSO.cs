using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Building Type List")]
public class BuildingTypeListSO : ScriptableObject
{
    public List<BuildingTypeSO> list;
}

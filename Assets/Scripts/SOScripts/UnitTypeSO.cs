using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Unit Type")]
public class UnitTypeSO : ScriptableObject
{
    public string nameString;
    public string description;
    public Transform unitPrefab;
    public int cost;
}

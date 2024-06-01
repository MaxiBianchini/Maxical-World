using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BuildingType")]
public class BuildingTypeSO : ScriptableObject
{
    [Tooltip("Tower Name")]
    public string nameString;
    [Tooltip("Tower prefab")]
    public Transform buildingPrefab;
    [Tooltip("Bullet prefab if applies")]
    public Transform bullet;
    [Tooltip("Amount of gold required to build this tower")]
    public int goldAmountCost;
    [Tooltip("Max health amount for this tower")]
    public int healthAmountMax;
    [Tooltip("Time needed to build this tower")]
    public int constructionTimerMax;

    
}

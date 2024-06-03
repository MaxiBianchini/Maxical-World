using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "Wave")]
public class WavesData : ScriptableObject
{
    [Header("Wave")] 
    public List<GameObject> groupList = new List<GameObject>();
    
    public void Spawn()
    {
        
    }

}

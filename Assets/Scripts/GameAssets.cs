using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;
    public static GameAssets Instance {
        get
        {
            if(instance == null)
            {
                instance = Resources.Load<GameAssets>("GameAssets");
            }
            return instance;
        }
         
    }

    public Transform pfArrowProjectile;
    public Transform pfArrowTower;
    public Transform pfGhostArrowTowerMesh;
    //public Transform pfArrowDestroyedParticles;
    //public Transform pfBuildingDestroyedParticles;
    //public Transform pfBuildingPlacedParticles;
    //public Transform pfBuildingConstruction;
}

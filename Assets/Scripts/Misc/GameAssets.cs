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
    public Transform pfCanonProjectile;
    public Transform pfTower;
    public Transform pfGhostArrowTowerMesh;
    public Transform pfHitGroundVFX;
    public Transform pfBuildingPlacedParticles;
    public Transform pfSmallFireVFX;
    public Transform pfMediumFireVFX;
    public Transform pfBigFireVFX;
    public Transform pfBuildingDestroyedParticles;
    public Transform pfCanonSplash;
    public Transform pfDeathEnemyVFX_1;
    //public Transform pfBuildingConstruction;
}

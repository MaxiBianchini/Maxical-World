using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private float spawnPointRadius = 2f;
    [SerializeField] private Material blueMaterial;
    [SerializeField] private Material redMaterial;
    [SerializeField] private LayerMask layerMask;

    private bool readyToBuild = false;
    private bool isBuilding = false;
    private bool canBuild;
    private Transform pfGhostTower;
    private Transform ghostTowerInstance;
    void Update()
    {
        if (!isBuilding && Input.GetKeyDown(KeyCode.E))
        {
            StartBuildingProcess();
        }

        if (isBuilding)
        {
            BuildingProcess();
        }
    }

    private void StartBuildingProcess()
    {
        readyToBuild = true;
        isBuilding = true;

        if (pfGhostTower == null)
        {
            pfGhostTower = GameAssets.Instance.pfGhostArrowTowerMesh;
        }
        
        ghostTowerInstance = Instantiate(pfGhostTower);
    }

    private void BuildingProcess()
    {
        CreateGhostBuildingMesh();

        if (CanSpawnBuilding(UtilsClass.GetMouseWorldPosition()))
        {

            ChangeGhostTowerMeshMaterial(blueMaterial);

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
            
                // TO DO: Hacer que solo se pueda construir cerca de un rango del player.
                //TO DO: call EnemyController add tower method --- Noe
                Tower.Create(UtilsClass.GetMouseWorldPosition());
            }
        }
        else
        {
            ChangeGhostTowerMeshMaterial(redMaterial);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            readyToBuild = false;
            isBuilding = false;

            Destroy(ghostTowerInstance.gameObject);
        }
    }


    private void ChangeGhostTowerMeshMaterial(Material newMaterial)
    {
        MeshRenderer[] towerMeshs = ghostTowerInstance.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mesh in towerMeshs)
        {
            mesh.material = newMaterial;
        }
    }

    private void CreateGhostBuildingMesh()
    {
        ghostTowerInstance.position = UtilsClass.GetMouseWorldPosition();
        ghostTowerInstance.position = new Vector3(ghostTowerInstance.position.x, 0f, ghostTowerInstance.position.z);
    }

    private bool CanSpawnBuilding(Vector3 position)
    {
        Transform torre = GameAssets.Instance.pfArrowTower;
        Collider[] colliderArray = Physics.OverlapBox(position, spawnPointRadius * Vector3.one,Quaternion.identity, layerMask);
        
        return colliderArray.Length == 0;
    }
}

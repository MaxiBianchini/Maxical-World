using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;
    [SerializeField] private float spawnPointRadius = 2f;
    [SerializeField] private Material blueMaterial;
    [SerializeField] private Material redMaterial;
    [SerializeField] private LayerMask layerMask;

    private bool readyToBuild = false;
    private bool isBuilding = false;
    private bool isOverTowerSpawnPoint;
    private Transform pfGhostTower;
    private Transform ghostTowerInstance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    void Update()
    {
        transform.position = UtilsClass.GetMouseWorldPosition();

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
        SetGhostBuildingMeshPosition();

        if (CanSpawnBuilding(UtilsClass.GetMouseWorldPosition()) && isOverTowerSpawnPoint)
        {
            ChangeGhostTowerMeshMaterial(blueMaterial);

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                // TO DO: Hacer que solo se pueda construir cerca de un rango del player.
                //TO DO: call EnemyController add tower method --- Noe
                //Tower.Create(UtilsClass.GetMouseWorldPosition());
                Tower.Create(ghostTowerInstance.position);

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

    public void SetTowerPosition(Vector3 spawnPosition, bool value)
    {
        isOverTowerSpawnPoint = value;
        if(ghostTowerInstance != null)
        {
            ghostTowerInstance.position = spawnPosition;
        }
    }

    public void SetTowerPosition(bool value)
    {
        isOverTowerSpawnPoint = false;
    }

        private void ChangeGhostTowerMeshMaterial(Material newMaterial)
    {
        MeshRenderer[] towerMeshs = ghostTowerInstance.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mesh in towerMeshs)
        {
            mesh.material = newMaterial;
        }
    }

    private void SetGhostBuildingMeshPosition()
    {
        if(!isOverTowerSpawnPoint)
        {
            ghostTowerInstance.position = UtilsClass.GetMouseWorldPosition();
            ghostTowerInstance.position = new Vector3(ghostTowerInstance.position.x, 0f, ghostTowerInstance.position.z);
        }
    }



    private bool CanSpawnBuilding(Vector3 position)
    {
        Transform torre = GameAssets.Instance.pfArrowTower;
        Collider[] colliderArray = Physics.OverlapBox(position, spawnPointRadius * Vector3.one,Quaternion.identity, layerMask);
        
        return colliderArray.Length == 0;
    }
}

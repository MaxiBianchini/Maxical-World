using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private float spawnPointRadius = 2f;

    private bool readyToBuild = false;
    private bool isBuilding = false;
    private Transform pfGhostTower;
    private Transform ghostTowerInstance;
    void Update()
    {
        //CanSpawnBuilding(UtilsClass.GetMouseWorldPosition());
        
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
    }

    private void BuildingProcess()
    {
        CreateGhostBuildingMesh();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (CanSpawnBuilding(UtilsClass.GetMouseWorldPosition()))
            {
                // TO DO: Hacer que detecte si hay algun objeto encima del mouse y alrededores al poner la torre
                // TO DO: Hacer que solo se pueda construir cerca de un rango del player.
                Tower.Create(UtilsClass.GetMouseWorldPosition());

                /*if (CanSpawnBuilding(UtilsClass.GetMouseWorldPosition(), out string errorMessage))
                {
                }
                */
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            readyToBuild = false;
            isBuilding = false;

            //---------------------------------------------------------
            // -------- REVISAR. NO SE PUEDE DESTRUIR EL PREFAB -------
            //---------------------------------------------------------
            Destroy(pfGhostTower.gameObject);
            Destroy(ghostTowerInstance.gameObject);
        }
    }

    private void CreateGhostBuildingMesh()
    {
        if(pfGhostTower == null)
        {
            pfGhostTower = GameAssets.Instance.pfGhostArrowTowerMesh;
            ghostTowerInstance = Instantiate(pfGhostTower);
        }

        ghostTowerInstance.position = UtilsClass.GetMouseWorldPosition();
        ghostTowerInstance.position = new Vector3(ghostTowerInstance.position.x, 0f, ghostTowerInstance.position.z);
    }

    private bool CanSpawnBuilding(Vector3 position)//, out string errorMessage)
    {
        bool canBuild = true;
        float a = spawnPointRadius;
        Vector3 radius = new Vector3(a, a, a);
        //BoxCollider boxCollider = buildingType.prefab.GetComponent<BoxCollider>();
        Transform torre = GameAssets.Instance.pfArrowTower;
        Debug.Log(torre.name);
        //BoxCollider boxCollider = torre.GetComponent<BoxCollider>();
        Collider[] colliderArray = Physics.OverlapBox(position, radius);
        foreach(Collider collider in colliderArray)
        {
            if (collider.gameObject.layer != LayerMask.NameToLayer("Ground"))
            {
                Debug.Log("Hay un objeto colisionando " + collider.name);
                //TO DO: Poner una sombre ghost en verde
                canBuild = false;
            }
            else
            {
                //TO DO: Poner una sombre ghost en rojo
                canBuild = true;
            }
            
        }
        Debug.Log("Return " + canBuild);
        return canBuild;
    }
}

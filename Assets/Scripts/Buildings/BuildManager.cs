using System;
using System.Collections;
using System.Collections.Generic;
using Enemys;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;
    [SerializeField] private float spawnPointRadius = 2f;
    [SerializeField] private Material blueMaterial;
    [SerializeField] private Material redMaterial;
    [SerializeField] private LayerMask layerMask;

   // [SerializeField] private bool readyToBuild = false;
    [SerializeField] private bool isBuilding = false;
    [SerializeField] private bool isOverTowerSpawnPoint;
    [SerializeField] private float maxBuildingDistance = 5f;


    //[SerializeField] private int inventoryGold;

    [SerializeField] private Image tooFarAwayToBuildImage;

    private Transform pfGhostTower;
    private Transform ghostTowerInstance;
    private Vector3 lastGhostTowerPosition;
    private PlayerController player;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        player = FindObjectOfType<PlayerController>();
    }

    private void Start()
    {
    }

    private void OnEnable()
    {
        player.onPlayerDeath += DisableBuildingMesh;
    }

    private void OnDisable()
    {
        player.onPlayerDeath -= DisableBuildingMesh;
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
      //  readyToBuild = true;
        

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

        Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();

        bool outOfRange = OutOfBuildingRangeDistance(mousePosition);

        if (CanSpawnBuilding(mousePosition) && isOverTowerSpawnPoint && !outOfRange)
        {
            //Apago mensaje de que esta lejos para construir
            GetComponentInChildren<TMP_Text>().enabled = false;

            ChangeGhostTowerMeshMaterial(blueMaterial);

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                // TO DO: Hacer que solo se pueda construir cerca de un rango del player.
                //TO DO: call EnemyController add tower method --- Noe
                //Tower.Create(UtilsClass.GetMouseWorldPosition());
                
                //Traigo el prefab de la torre y su costo en oro para validarlo
                Transform pfTower = GameAssets.Instance.pfTower;
                int towerCost = pfTower.GetComponent<BuildingTypeHolder>().buildingType.goldAmountCost;
                if (CanAffordCost(towerCost))
                {
                    var tower = Tower.Create(ghostTowerInstance.position, pfTower);
                    EnemyController.Instance.AddTower(tower.gameObject);
                }

            }
        }
        else
        {
            ChangeGhostTowerMeshMaterial(redMaterial);
            //GetComponentInChildren<TMP_Text>().enabled = outOfRange;
            Debug.Log(outOfRange);
            tooFarAwayToBuildImage.enabled = outOfRange;


        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DisableBuildingMesh();
        }
    }

    private void DisableBuildingMesh()
    {
        isBuilding = false;
        //GetComponentInChildren<TMP_Text>().enabled = false;
        tooFarAwayToBuildImage.enabled = false;
        Destroy(ghostTowerInstance.gameObject);
    }

    private void DisableBuildingMesh(object sender, EventArgs e)
    {
        isBuilding = false;
        //GetComponentInChildren<TMP_Text>().enabled = false;
        tooFarAwayToBuildImage.enabled = false;

        if (ghostTowerInstance)
        {
            Destroy(ghostTowerInstance.gameObject);
        }
    }

    private bool OutOfBuildingRangeDistance(Vector3 mousePosition)
    {
        Vector3 playerPosition = FindObjectOfType<PlayerController>().transform.position;
        float distance = Vector3.Distance(mousePosition, playerPosition);

        return distance > maxBuildingDistance;
    }

    //Chequea si hay oro suficiente para construir la torre
    private bool CanAffordCost(int towerCost)
    {
        float inventoryGold = CoinManager.Instance.GetTotalCoins();
        if(inventoryGold >= towerCost)
        {
            CoinManager.Instance.LessCoinMount(towerCost);
            return true;
        }
        else
        {
            Debug.Log("No tengo plata para construir");
            return false;
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
            PlayerController player = FindObjectOfType<PlayerController>();
            float distance = Vector3.Distance(ghostTowerInstance.position, player.transform.position);

                lastGhostTowerPosition = ghostTowerInstance.position;
                ghostTowerInstance.position = lastGhostTowerPosition;
                

        }
    }

   



    private bool CanSpawnBuilding(Vector3 position)
    {

        Transform torre = GameAssets.Instance.pfTower;
        Collider[] colliderArray = Physics.OverlapBox(position, spawnPointRadius * Vector3.one,Quaternion.identity, layerMask);
        
        return colliderArray.Length == 0;
    }
}

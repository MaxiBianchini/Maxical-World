using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawnPoint : MonoBehaviour
{
    //This class handles the SpawnPoint object

    private List<MeshRenderer> meshRenderers;
    private Building building;
    private TowerHealthSystem towerHealthSystem;

    private void OnEnable()
    {
        if(towerHealthSystem != null)
        {
            //Suscription to towerDeathEvent
            towerHealthSystem.onTowerDeath += HandleTowerDeath;
        }
    }

    private void OnDisable()
    {
        if (towerHealthSystem != null)
        {
            towerHealthSystem.onTowerDeath -= HandleTowerDeath;
        }
    }

    //When tower dies, show again the spawnPoint
    private void HandleTowerDeath(object sender, EventArgs e)
    {
        ShowHideAllMeshRenderers(true);
    }

   

    private void Start()
    {
        meshRenderers = new List<MeshRenderer>();
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        //Add all the renderes to a list to hide/show them in any situation
        foreach (MeshRenderer renderer in renderers)
        {
            meshRenderers.Add(renderer);
        }
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        //If a tower is created on that position. Then hide spawnPoint.
        building = collision.GetComponent<Building>();
        if (building)
        {
            towerHealthSystem = building.GetComponent<TowerHealthSystem>();
            //When a tower is created, suscribe to its TowerHealthSystem death Event.
            OnEnable();
            //Hide all renderers
            ShowHideAllMeshRenderers(false);
        }

    }
    private void OnTriggerExit(Collider collision)
    {
        //If tower dies, show the spawnPoint
        building = collision.GetComponent<Building>();
        if (building)
        {
            ShowHideAllMeshRenderers(true);
        }
    }

    //Show or hide all mesh renderers in TowerSpawnPoint
    private void ShowHideAllMeshRenderers(bool value)
    {
        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.enabled = value;
        }
    }
}

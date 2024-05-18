using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawnPoint : MonoBehaviour
{
    private List<MeshRenderer> meshRenderers;

    private void OnEnable()
    {
        TowerHealthSystem.OnTowerDeath += HandleTowerDeath;
    }

    private void OnDisable()
    {
        TowerHealthSystem.OnTowerDeath -= HandleTowerDeath;
    }

    private void HandleTowerDeath(TowerHealthSystem tower)
    {
        // Lógica para manejar la aparición de la torre cuando esta muere
        // Por ejemplo, establecer la posición de aparición o iniciar un temporizador para reaparecer
        //Vector3 spawnPosition = tower.transform.position;
        ShowHideAllMeshRenderers(true);


    }

   

    private void Start()
    {
        meshRenderers = new List<MeshRenderer>();
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            meshRenderers.Add(renderer);
        }
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.GetComponent<Building>())
        {
            ShowHideAllMeshRenderers(false);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Building>())
        {
            ShowHideAllMeshRenderers(true);
        }
    }
    private void ShowHideAllMeshRenderers(bool value)
    {
        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.enabled = value;
        }
    }
}

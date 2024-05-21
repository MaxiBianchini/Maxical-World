using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private BuildingTypeSO buildingType;
    private TowerHealthSystem healthSystem;
    private Tower tower;

    private void Start()
    {
        buildingType = GetComponent<BuildingTypeHolder>().buildingType;
        healthSystem = GetComponent<TowerHealthSystem>();
        tower = GetComponent<Tower>();
        //TO DO:
        //Eventos para designar vida maxima
        //Evento para morir
        //Evento para ser dañada
        //Evento para curarse

    }


    public void DustEvent()
    {
        Transform dustPrefab = GameAssets.Instance.pfBuildingPlacedParticles;
        Transform dustGameObject = Instantiate(dustPrefab, transform.position, Quaternion.identity, this.transform);
        dustGameObject.eulerAngles = new Vector3(-90f, 0, 0);
        dustGameObject.GetComponent<ParticleSystem>().Play();
        
    }

    public void ReadyToAttack()
    {
        tower.CanAttack = true;
    }


    



    private void HealthSystem_OnDamaged()
    {

    }

    private void HealthSystem_OnDied()
    {

    }
}

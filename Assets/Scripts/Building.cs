using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private BuildingTypeSO buildingType;
    private TowerHealthSystem healthSystem;

    private void Start()
    {
        buildingType = GetComponent<BuildingTypeHolder>().buildingType;
        healthSystem = GetComponent<TowerHealthSystem>();
        //TO DO:
        //Eventos para designar vida maxima
        //Evento para morir
        //Evento para ser dañada
        //Evento para curarse

    }

    private void HealthSystem_OnDamaged()
    {

    }

    private void HealthSystem_OnDied()
    {

    }
}

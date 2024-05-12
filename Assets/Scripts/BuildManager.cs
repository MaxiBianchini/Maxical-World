using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
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

    private bool CanSpawnBuilding(Vector3 position, out string errorMessage)
    {
        Debug.Log("Entro al metodo para chequear la posicion de la torre");
        //BoxCollider boxCollider = buildingType.prefab.GetComponent<BoxCollider>();
        Transform torre = GameAssets.Instance.pfArrowTower;

        BoxCollider boxCollider = torre.GetComponent<BoxCollider>();
        Collider[] colliderArray = Physics.OverlapBox(position + boxCollider.center, boxCollider.size / 2);

        bool isAreaClear = colliderArray.Length == 0;
        Debug.Log(isAreaClear);
        if (!isAreaClear)
        {
            errorMessage = "Area is not clear!";
            return false;

        }
        else
        {
            errorMessage = "Area is clear to build!";
            return true;
        }



    }
}

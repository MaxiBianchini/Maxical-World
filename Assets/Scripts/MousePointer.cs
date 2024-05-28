using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointer : MonoBehaviour
{
    private Vector3 towerSpawnPoint;
    private bool isOverTowerSpawnPoint;


    private void Start()
    {
        towerSpawnPoint = Vector3.zero;
    }
    void Update()
    {
        transform.position = UtilsClass.GetMouseWorldPosition();
        transform.position = new Vector3(transform.position.x,0f,transform.position.z);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.GetComponent<TowerSpawnPoint>())
        {
            isOverTowerSpawnPoint = true;
            towerSpawnPoint = collision.transform.position;
            BuildManager.Instance.SetTowerPosition(towerSpawnPoint, isOverTowerSpawnPoint);
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<TowerSpawnPoint>())
        {
            isOverTowerSpawnPoint = false;
            towerSpawnPoint = Vector3.zero;
            BuildManager.Instance.SetTowerPosition(isOverTowerSpawnPoint);
        }
    }
}

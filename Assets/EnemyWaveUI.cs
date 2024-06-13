using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyWaveUI : MonoBehaviour
{
    private Camera mainCamera;
    private RectTransform enemyClosestPositionIndicator;
    private void Awake()
    {
        enemyClosestPositionIndicator = transform.Find("enemyClosestPositionIndicator").GetComponent<RectTransform>();

    }

    private void Start()
    {
        mainCamera = Camera.main;
    }
    private void Update()
    {
        HandleEnemyClosestPositionIndicator();
    }

    private void HandleEnemyClosestPositionIndicator()
    {
        float targetMaxRadius = 9999f;
        Collider[] colliderArray = Physics.OverlapSphere(mainCamera.transform.position, targetMaxRadius);

        Transform targetEnemy = null;
        foreach (Collider collider in colliderArray)
        {
            if (collider.CompareTag("Enemy"))
            {
                Transform enemy = collider.transform;
                if (enemy != null)
                {
                    //it's an enemy
                    if (targetEnemy == null)
                    {
                        targetEnemy = enemy;
                    }
                    else
                    {
                        if (Vector3.Distance(transform.position, enemy.transform.position) <
                            Vector3.Distance(transform.position, targetEnemy.transform.position))
                        {
                            //Building is closer!
                            targetEnemy = enemy;
                        }
                    }
                }
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public static Tower Create(Vector3 position, Transform pfTower)
    {
        AudioManager.Instance.PlayEffect("Build");
        Transform TowerTransform = Instantiate(pfTower, position, Quaternion.identity);
        Tower tower = TowerTransform.GetComponent<Tower>();
        return tower;
    }


    [SerializeField] private float shootTimerMax;
    [SerializeField] private float targetMaxRadius = 20f;
    [SerializeField] private float towerAccuracy = .75f;

    [SerializeField] private bool canAttack = false;
    public bool CanAttack { get { return canAttack; } set { canAttack = value; } }
    
    private float shootTimer;
    private float lookForTargetTimer;
    private float lookForTargetTimerMax = 0.2f;
    private ProjectileSpawnPoint projectileSpawnPoint;
    [SerializeField] private Transform projectileSpawnPosition;
    private Transform _targetEnemy;
   // private Transform _enemy;
    private BuildingTypeHolder buildingTypeHolder;

    private void Awake()
    {
        buildingTypeHolder = GetComponent<BuildingTypeHolder>();
        projectileSpawnPoint = GetComponentInChildren<ProjectileSpawnPoint>();
        
    }

    private void Update()
    {
        HandleTargeting();
        HandleShooting();
    }


    private void HandleShooting()
    {
        shootTimer -= Time.deltaTime;
        if(shootTimer < 0)
        {
            shootTimer += shootTimerMax;
            if(_targetEnemy != null && canAttack)
            {
                AudioManager.Instance.PlayEffect("Cannon Ball");
                Projectile.Create(projectileSpawnPosition.transform.position, _targetEnemy, towerAccuracy, projectileSpawnPoint.transform.rotation, buildingTypeHolder.buildingType);
            }
        }
    }

    private void HandleTargeting()
    {
        //In order to avoid consuming pc resources, a timer will call the function lookForTargets in a randomRange of time.
        lookForTargetTimer -= Time.deltaTime;
        if (lookForTargetTimer < 0f)
        {
            lookForTargetTimer += lookForTargetTimerMax;
            LookForTargets();
        }
    }

    //noe modif
    private void LookForTargets()
    {
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, targetMaxRadius);
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider collider in colliderArray)
        {

            if (collider.CompareTag("Enemy"))
            {
                float distanceToEnemy = Vector3.Distance(transform.position, collider.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestEnemy = collider.transform;
                    closestDistance = distanceToEnemy;
                }
            }
        }

        if(closestEnemy != null )
        {
            _targetEnemy = closestEnemy;
            projectileSpawnPoint.RotateTowardsEnemy(_targetEnemy);
            GetComponentInChildren<TowerRotater>().RotateTowardsEnemy(_targetEnemy);

        }
        else
        {
            _targetEnemy = null;
        }
              
        if(_targetEnemy != null && Vector3.Distance(transform.position, _targetEnemy.position) > targetMaxRadius)
        {
            _targetEnemy = null;
        }
       


    }
}

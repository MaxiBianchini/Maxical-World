using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public static Tower Create(Vector3 position)
    {
        Transform pfArrowTower = GameAssets.Instance.pfArrowTower;
        Transform TowerTransform = Instantiate(pfArrowTower, position, Quaternion.identity);

        Tower tower = TowerTransform.GetComponent<Tower>();
        return tower;
    }


    [SerializeField] private float shootTimerMax;
    [SerializeField] private float targetMaxRadius = 20f;
    [SerializeField] private float towerAccuracy = .75f;

    public bool CanAttack { get { return canAttack; } set { canAttack = value; } }
    private bool canAttack = false;
    private float shootTimer;
    private EnemyTestLeo targetEnemy;
    private float lookForTargetTimer;
    private float lookForTargetTimerMax = 0.2f;

    private Vector3 projectileSpawnPosition;

    //noe modif
    private Transform _targetEnemy;
    private Transform _enemy;

    private void Start()
    {
        projectileSpawnPosition = transform.Find("ProjectileSpawnPosition").position;
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
                ArrowProjectile.Create(projectileSpawnPosition, _targetEnemy, towerAccuracy);
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
        foreach (Collider collider in colliderArray)
        {
            
            if (collider.CompareTag("Enemy"))
            {
                Debug.Log($"Enemy {collider.name}");
                _enemy = collider.transform;
                if (_targetEnemy == null)
                {
                    _targetEnemy = _enemy;
                }
                else
                {
                    if (Vector3.Distance(transform.position, _enemy.transform.position) <
                        Vector3.Distance(transform.position, _targetEnemy.transform.position))
                    {
                        //Enemy is on range
                        _targetEnemy = _enemy;
                    }
                }
                
            }
            
            
            
            
           /* Leo original script
            EnemyTestLeo enemy = collider.GetComponent<EnemyTestLeo>();
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
                        //Enemy is on range
                        targetEnemy = enemy;
                    }
                }
            }
            */
        }


    }
}

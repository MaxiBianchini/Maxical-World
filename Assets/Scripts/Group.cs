using System.Collections;
using System.Collections.Generic;
using Enemys;
using Enemys.Data;
using UnityEngine;

public class Group : MonoBehaviour
{
    [Header("Destroyer")] 
    [SerializeField] private GameObject destroyerPrefab;
    [SerializeField] private int destroyerAmount;
    [SerializeField] private float destroyerHealth;
    [SerializeField] private float destroyerDamage;
    [SerializeField] private float destroyerSpeed;
    [SerializeField] private int destroyerValue;

    [Header("Ranger")]
    [SerializeField] private GameObject rangerPrefab;
    [SerializeField] private int rangerAmount;
    [SerializeField] private float rangerHealth;
    [SerializeField] private float rangerDamage;
    [SerializeField] private float rangerSpeed;
    [SerializeField] private int rangerValue;
        
    [Header("Chaser")]
    [SerializeField] private GameObject chaserPrefab;
    [SerializeField] private int chaserAmount;
    [SerializeField] private float chaserHealth;
    [SerializeField] private float chaserDamage;
    [SerializeField] private float chaserSpeed;
    [SerializeField] private int chaserValue;
        
    [Header("Spawns Points")]
    [SerializeField] private List<Transform> spawnPointList = new List<Transform>();
        
    [Header("Targets Data")] 
    [SerializeField] private EnemyTarget destroyerTargets;
    [SerializeField] private EnemyTarget rangerTargets;
    [SerializeField] private EnemyTarget chaserTargets;

    private float _spawnTime;
    private GameObject _player;
    private Coroutine _waveTimerCoroutine;
    private IEnemy _enemy;
    private GameObject _newEnemy;
    private bool _isEnemiesAlive;
        

    private enum EnemyType {
        Destroyer,
        Ranger,
        Chaser
    }


    //metodos
        
    private void SpawnEnemy(EnemyType type)
    {
        Vector3 spawn;
        GameObject prefab, target;
        int amount, value;
        float health, damage, speed;

        switch (type) {
            case EnemyType.Destroyer:
                prefab = destroyerPrefab;
                target = destroyerTargets.primaryTarget;
                amount = destroyerAmount;
                health = destroyerHealth;
                damage = destroyerDamage;
                speed = destroyerSpeed;
                value = destroyerValue;
                break;
            case EnemyType.Ranger:
                prefab = rangerPrefab;
                target = rangerTargets.primaryTarget;
                amount = rangerAmount;
                health = rangerHealth;
                damage = rangerDamage;
                speed = rangerSpeed;
                value = rangerValue;

                break;
            case EnemyType.Chaser:
                prefab = chaserPrefab;
                target = chaserTargets.primaryTarget;
                amount = chaserAmount;
                health = chaserHealth;
                damage = chaserDamage;
                speed = chaserSpeed;
                value = chaserValue;

                break;
            default:
                Debug.LogError("SpawnEnemy - Error tipo de enemigo");
                prefab = null;
                target = null;
                amount = 0;
                health = 0;
                damage = 0;
                speed = 0;
                value = 0;
                break;
        }
            
        for (int i = 0; i < spawnPointList.Count; i++) {
            for (int j = 0; j < amount; j++)
            {
                //tiempo para que spawnee el siguiente? ver milanote
                _newEnemy = Instantiate(prefab, SpawnsPoints(i), Quaternion.identity);
                spawn = spawnPointList[i].position;
                _enemy = _newEnemy.GetComponent<IEnemy>();
                _enemy.Initialize(health, damage, speed, value);
                _newEnemy.GetComponent<IEnemy>().SetDestination(ClosestTarget(spawn, target));
                EnemyController.Instance.enemiesList.Add(_newEnemy);
            }
        }
    }
    
    
        
    private Vector3 SpawnsPoints(int index)
    {
        Vector3 spawn = spawnPointList[index].position;
        return spawn;
    }
        
    private GameObject ClosestTarget(Vector3 spawnPoint, GameObject target)
    {
        GameObject closestGameObject = null;
        float closestDistance = Mathf.Infinity;
        float currentDistance;
            
        switch (target.tag)
        {
            case "Door":
                foreach (var door in EnemyController.Instance.Doors)
                {
                    currentDistance = Vector3.Distance(spawnPoint, door.transform.position);
                    if (currentDistance < closestDistance)
                    {
                        closestDistance = currentDistance;
                        closestGameObject = door;
                    }
                }
                break;
                
            case "Tower":
                foreach (var tower in EnemyController.Instance.Towers)
                {
                    currentDistance = Vector3.Distance(spawnPoint, tower.transform.position);
                    if (currentDistance < closestDistance)
                    {
                        closestDistance = currentDistance;
                        closestGameObject = tower;
                    }
                }
                break;
                
            case "Player":
                _player = EnemyController.Instance.Player;
                closestGameObject = _player;
                break;
        }
            
        return closestGameObject;
    }

    public void SpawnAllEnemies()
    {
        SpawnEnemy(EnemyType.Destroyer);
        SpawnEnemy(EnemyType.Chaser);
        SpawnEnemy(EnemyType.Ranger);
    }

}
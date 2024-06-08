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
    private GameObject _nexo;
    private Coroutine _waveTimerCoroutine;
    private IEnemy _enemy;
    private GameObject _newEnemy;
    private bool _isEnemiesAlive;
    
    private Coroutine _destroyerCoroutine;
    private Coroutine _rangerCoroutine;
    private Coroutine _chaserCoroutine;
    private Coroutine _allEnemiesCoroutine;
        

    private enum EnemyType {
        Destroyer,
        Ranger,
        Chaser
    }


    //metodos
        
    private IEnumerator SpawnEnemiesOfType(EnemyType type)
{
    Vector3 spawn;
    GameObject prefab, target;
    int amount, value;
    float health, damage, speed;

    switch (type) {
        case EnemyType.Destroyer:
            prefab = destroyerPrefab;
            target = TargetFilter(type);
            amount = destroyerAmount;
            health = destroyerHealth;
            damage = destroyerDamage;
            speed = destroyerSpeed;
            value = destroyerValue;
            break;
        case EnemyType.Ranger:
            prefab = rangerPrefab;
            target = TargetFilter(type);
            amount = rangerAmount;
            health = rangerHealth;
            damage = rangerDamage;
            speed = rangerSpeed;
            value = rangerValue;
            break;
        case EnemyType.Chaser:
            prefab = chaserPrefab;
            target = TargetFilter(type);
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
            yield return new WaitForSeconds(1f);
            _newEnemy = Instantiate(prefab, SpawnsPoints(i), Quaternion.identity);
            spawn = spawnPointList[i].position;
             _enemy = _newEnemy.GetComponent<IEnemy>();
            _enemy.Initialize(health, damage, speed, value);
            _enemy.SetDestination(ClosestTarget(spawn, TargetFilter(type)));
            EnemyController.Instance.enemiesList.Add(_newEnemy);
        }
    }
}
    
    
    //que necesito? filtrar si existe un target inicial, y si no, avanzar con el segundo target, filtrarlo y si no, avanzar con el ultimo.
    
    //1. que a ClosestTarget() le pase una lista? con todos los targets que puede llegar a tener y luego en el metodo closestarget evalue esa lista?
    //2. Hacer un metodo que reciba el tipo de enemigo y verifique los targets y devuelva un target (que se haga en el switch) ++++++++++++++
    //3. 

    private GameObject TargetFilter(EnemyType type)
    {
        GameObject target;
        
        switch (type) {
            case EnemyType.Destroyer:
                if (EnemyController.Instance.Doors.Count > 0)
                {
                    target = destroyerTargets.primaryTarget;
                }
                else
                {
                    target = destroyerTargets.finalTarget;
                }
                break;
            case EnemyType.Ranger:
                if (EnemyController.Instance.Towers.Count > 0)
                {
                    target = rangerTargets.primaryTarget;
                }
                else if (EnemyController.Instance.Doors.Count > 0)
                {
                    target = rangerTargets.secondaryTarget;
                }
                else
                {
                    target = rangerTargets.finalTarget;
                }
                break;
            case EnemyType.Chaser:
                if (EnemyController.Instance.Player != null)
                {
                    target = chaserTargets.primaryTarget;
                }
                else if (EnemyController.Instance.Doors.Count > 0)
                {
                    target = chaserTargets.secondaryTarget;
                }
                else
                {
                    target = chaserTargets.finalTarget;
                }
                break;
            default:
                target = EnemyController.Instance.Nexo;
                break;
        }
        return target;
    }
    
    private Vector3 SpawnsPoints(int index)
    {
        Vector3 spawn = spawnPointList[index].position;
        return spawn;
    }
        
    private GameObject ClosestTarget(Vector3 spawnPoint, GameObject target)
    {
        // en el momento que se crea un enemigo se le setea el target ranger = door, pero si no hay ninguna door esta igual le setea la door. Tengo que hacer que vaya al siguiente tipo de target
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
            
            case "Nexo":
                _nexo = EnemyController.Instance.Nexo;
                closestGameObject = _nexo;
                break;
        }
            
        return closestGameObject;
    }
    
    public void StopAllSpawning()
    {
        if (_allEnemiesCoroutine != null)
        {
            StopCoroutine(_allEnemiesCoroutine);
            _allEnemiesCoroutine = null;
        }

        if (_destroyerCoroutine != null)
        {
            StopCoroutine(_destroyerCoroutine);
            _destroyerCoroutine = null;
        }

        if (_rangerCoroutine != null)
        {
            StopCoroutine(_rangerCoroutine);
            _rangerCoroutine = null;
        }

        if (_chaserCoroutine != null)
        {
            StopCoroutine(_chaserCoroutine);
            _chaserCoroutine = null;
        }
    }

    public void SpawnAllEnemies()
    {
        StopAllSpawning();
        _allEnemiesCoroutine = StartCoroutine(SpawnAllEnemiesCoroutine());
    }
    private IEnumerator SpawnAllEnemiesCoroutine()
    {
        _destroyerCoroutine = StartCoroutine(SpawnEnemiesOfType(EnemyType.Destroyer));
        yield return _destroyerCoroutine;

        _rangerCoroutine = StartCoroutine(SpawnEnemiesOfType(EnemyType.Ranger));
        yield return _rangerCoroutine;

        _chaserCoroutine = StartCoroutine(SpawnEnemiesOfType(EnemyType.Chaser));
        yield return _chaserCoroutine;
    }

}
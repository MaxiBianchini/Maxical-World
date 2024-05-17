using System.Collections.Generic;
using Enemys.Data;
using UnityEngine;

namespace Enemys
{
    public class WaveManager : MonoBehaviour
    {
        [Header("Destroyer")] 
        [SerializeField] private GameObject destroyerPrefab;
        [SerializeField] private int destroyerAmount;
        [SerializeField] private float destroyerHealth;
        [SerializeField] private float destroyerDamage;
        [SerializeField] private float destroyerSpeed;

        [Header("Ranger")]
        [SerializeField] private GameObject rangerPrefab;
        [SerializeField] private int rangerAmount;
        [SerializeField] private float rangerHealth;
        [SerializeField] private float rangerDamage;
        [SerializeField] private float rangerSpeed;
        
        [Header("Chaser")]
        [SerializeField] private GameObject chaserPrefab;
        [SerializeField] private int chaserAmount;
        [SerializeField] private float chaserHealth;
        [SerializeField] private float chaserDamage;
        [SerializeField] private float chaserSpeed;
        
        [Header("Spawns Points")]
        [SerializeField] private List<Transform> spawnPointList = new List<Transform>();

        [Header("Targets Data")] 
        [SerializeField] private EnemyTarget destroyerTargets;
        [SerializeField] private EnemyTarget rangerTargets;
        [SerializeField] private EnemyTarget chaserTargets;
        
        
        private float _spawnTime;
        

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
            int amount;
            float health, damage, speed;

            switch (type) {
                case EnemyType.Destroyer:
                    prefab = destroyerPrefab;
                    target = destroyerTargets.primaryTarget;
                    amount = destroyerAmount;
                    health = destroyerHealth;
                    damage = destroyerDamage;
                    speed = destroyerSpeed;
                    break;
                case EnemyType.Ranger:
                    prefab = rangerPrefab;
                    target = rangerTargets.primaryTarget;
                    amount = rangerAmount;
                    health = rangerHealth;
                    damage = rangerDamage;
                    speed = rangerSpeed;
                    break;
                case EnemyType.Chaser:
                    prefab = chaserPrefab;
                    target = chaserTargets.primaryTarget;
                    amount = chaserAmount;
                    health = chaserHealth;
                    damage = chaserDamage;
                    speed = chaserSpeed;
                    break;
                default:
                    Debug.LogError("SpawnEnenmy - Error tipo de enemigo");
                    prefab = destroyerPrefab;
                    target = null;
                    amount = 0;
                    health = 0;
                    damage = 0;
                    speed = 0;
                    break;
            }
            
            for (int i = 0; i < spawnPointList.Count; i++) {
                for (int j = 0; j < amount; j++)
                {
                    GameObject newEnemy = Instantiate(prefab, SpawnsPoints(i), Quaternion.identity); //todo ver newENemy que hace xd
                    spawn = spawnPointList[i].position;
                    prefab.GetComponent<IEnemy>().SetDestination(ClosestTarget(spawn, target));
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
            }

            return closestGameObject;
        }
        
    }
}

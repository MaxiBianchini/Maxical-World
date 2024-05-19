using System;
using System.Collections.Generic;
using Enemys.Data;
using Enemys.Destroyer;
using UnityEngine;

namespace Enemys
{
    public class EnemyController : MonoBehaviour
    {
        public static EnemyController Instance { get; private set; }
        public static event Action DoorDestroyedEvent;
        
        public GameObject Player => _player; 
        public GameObject Nexo => _nexo; 
        public IReadOnlyList<GameObject> Towers => _towerList.AsReadOnly();
        public IReadOnlyList<GameObject> Doors => _doorList.AsReadOnly();
        
        private readonly List<GameObject> _towerList = new List<GameObject>();
        private readonly List<GameObject> _doorList = new List<GameObject>();
        private GameObject _player; //si fuera multijugador esto seria una lista?
        private GameObject _nexo; // si hubiera mas de uno cambiarlo a lista

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            DoorDestroyedEvent = null;
        }

        private void Start()
        {
            FindAndAddAllTowers();
            FindAndAddAllDoors();
            FindAndAddPlayer();
            FindAndAddNexo();
           
            
           // DebugLists(); //borrar cuando chequee que funciona
        }
        
        private void FindAndAddAllTowers()
        {
            GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
            foreach (GameObject tower in towers)
            {
                AddTower(tower);
            }
        } 

        private void FindAndAddAllDoors()
        {
            GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
            foreach (GameObject door in doors)
            {
                AddDoor(door);
            }
        }
        
        private void FindAndAddPlayer()
        {
            _player = GameObject.FindWithTag("Player");
        }
        private void FindAndAddNexo()
        {
            _nexo = GameObject.FindWithTag("Nexo");
        }

        public void AddTower(GameObject tower)
        {
            _towerList.Add(tower);
        }

        public void RemoveTower(GameObject tower)
        {
            _towerList.Remove(tower);
        }

        public void AddDoor(GameObject door)
        {
            _doorList.Add(door);
        }

        public void RemoveDoor(GameObject door)
        {
            _doorList.Remove(door);
        }
        
        public static void OnDoorDestroyed()
        {
            DoorDestroyedEvent?.Invoke();
        }
        
        
        
        private void DebugLists() //borrar
        {
            Debug.Log("Torres: ");
            foreach (GameObject tower in _towerList)
            {
                Debug.Log(tower.name);
            }

            Debug.Log("Puertas: ");
            foreach (GameObject door in _doorList)
            {
                Debug.Log(door.name);
            }
        }

    }
}

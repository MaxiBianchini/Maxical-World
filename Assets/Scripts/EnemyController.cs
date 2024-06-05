using System;
using System.Collections.Generic;
using Enemys.Destroyer;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController Instance { get; private set; }
    public static event Action DoorDestroyedEvent;
        
    public GameObject Player => _player;
    public GameObject Nexo => _nexo;
    public List<GameObject> enemiesList = new List<GameObject>(); //todo cambiar la estructura para que solo se modifique por metodos
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
        DoorDestroyedEvent += HandleDoorDestroyed;

        FindAndAddAllTowers();
        FindAndAddAllDoors();
        FindAndAddPlayer();
        FindAndAddNexo();
           
            
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
  
    private void OnDestroy()
    {
        DoorDestroyedEvent -= HandleDoorDestroyed;

    }
        
    private void HandleDoorDestroyed()
    {
        foreach (DestroyerMovement destroyer in FindObjectsOfType<DestroyerMovement>())
        {
            destroyer.SetDestination(Nexo);
        }
    }
        
}
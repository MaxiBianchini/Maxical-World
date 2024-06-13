using System;
using System.Collections;
using System.Collections.Generic;
using Enemys;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    //lista de waves a spawnear (despues cambiar a scriptable obj)
    [SerializeField] private List<GameObject> waveList = new List<GameObject>();
    [SerializeField] private float timeBetweenWaves;
    
    private float _waveTimer;
    private bool _win = false;

    public event EventHandler onWaveSpawned;

    private void Start()
    {
        _waveTimer = timeBetweenWaves;
    }


    void Update()
    {
        NextSpawn();
    }

    private void NextSpawn() 
    {
        if (EnemyController.Instance.enemiesList.Count > 0)
        {
            return;
        }
    
        if (_waveTimer > 0)
        {
            _waveTimer -= Time.deltaTime;
        }

        if (_waveTimer <= 0 && EnemyController.Instance.enemiesList.Count <= 0)
        {
            SpawnWave();
            onWaveSpawned?.Invoke(this, EventArgs.Empty);
            _waveTimer = timeBetweenWaves;
        }
        
    }

    private void SpawnWave()
    {
        if (EnemyController.Instance.enemiesList.Count <= 0)
        {
            if (waveList.Count > 0)
            {
                waveList[0].GetComponent<Group>().SpawnAllEnemies();
                waveList.RemoveAt(0);
                _waveTimer = timeBetweenWaves;

            }
            else
            {
                Debug.Log("No hay más waves WIN");
                _win = true;
            }
        }
    }

    public float GetWaveTimer()
    {
        return _waveTimer;
    }

    public bool GetWinBool()
    {
        return _win;
    }

}

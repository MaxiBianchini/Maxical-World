using System;
using Enemys;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class SpawnEnemyUI : MonoBehaviour
    {
        [SerializeField] private WaveManager waveManager;
        [SerializeField] private Text text;

        private float _time;
        private void Update()
        {
            _time = waveManager.GetWaveTimer();
            
            if (EnemyController.Instance.enemiesList.Count > 0)
            {
                text.text = $"Under attack!";
            }
            else
            {
                text.text = $"Next wave: {_time:0}";
            }
        }
        
    }
}

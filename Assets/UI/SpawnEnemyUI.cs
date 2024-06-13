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
        [SerializeField] private Image underAttackimg;

        private float _time;
        private void Update()
        {
            _time = waveManager.GetWaveTimer();
            
            if (EnemyController.Instance.enemiesList.Count > 0)
            {
                underAttackimg.enabled = true;
                GameManager.instance.ShowImage(underAttackimg);
            }
            else
            {
                GameManager.instance.StartFadeOutImageRutine(underAttackimg);
                text.text = $"Next wave: {_time:0}";
            }
        }
        
    }
}

using Enemys;
using UnityEngine;

namespace UI
{
    public class SpawnEnemyButton : MonoBehaviour
    {
        [SerializeField] private WaveManager waveManager;
    
        public void OnSpawnEnemyButton()
        {
            if (EnemyController.Instance.enemiesList.Count == 0)
            {
                waveManager.SpawnAllEnemies();
                Debug.Log($"SE SPAWNEARON ENEMIGOS");
            }
            else
            {
                Debug.Log($"AUN QUEDAN ENEMIGOS VIVOS");
            }
            
        }
        
        public void ShowEnemyHealth()
    }
}

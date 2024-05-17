using UnityEngine;
using UnityEngine.Serialization;

namespace Enemys.Data
{
    [CreateAssetMenu(fileName = "EnemySettings", menuName = "Enemy/Settings")]
    public class EnemySettings : ScriptableObject
    {
        [Header("Destroyer Settings")] 
        public float destroyerHealth;
        public float destroyerDamage;
        
        [Header("Ranger Settings")] 
        public float rangerHealth;
        public float rangerDamage;
       
        [Header("Chaser Settings")] 
        public float chaserHealth;
        public float chaserDamage;
        public float chaserVelocity;
    }
}

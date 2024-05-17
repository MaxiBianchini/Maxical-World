using UnityEngine;

namespace Enemys.Data
{
    [CreateAssetMenu(fileName = "EnemyTarget", menuName = "Enemy/Target")]
    public class EnemyTarget : ScriptableObject
    {
        [Header("Targets")]
        public GameObject primaryTarget;
        public GameObject secondaryTarget;
        public GameObject finalTarget;
    }
}

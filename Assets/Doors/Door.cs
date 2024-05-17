using Common;
using Enemys;
using UnityEngine;
using UnityEngine.AI;

namespace Doors
{
    public class Door : MonoBehaviour, IDamageable
    {
        public float health;
        private NavMeshObstacle _navMesh;
        
        void Start()
        {
            _navMesh = GetComponent<NavMeshObstacle>();
            _navMesh.carving = true;
        }

        public void TakeDamage(float amount)
        {
            health -= amount;
            if (health <= 0)
            {
                Death();
            }
        }
        private void OnDestroy()
        {
            EnemyController.OnDoorDestroyed();
        }

        private void Death()
        {
            _navMesh.carving = false;
            Destroy(gameObject);
        }
    }
}

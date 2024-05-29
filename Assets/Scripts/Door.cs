using Common;
using Enemys;
using UnityEngine;
using UnityEngine.AI;

namespace Doors
{
    public class Door : MonoBehaviour, IDamageable
    {
        [SerializeField] private float health;
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
           // Debug.Log($"DOOR {gameObject.name} recibio  {amount} damage");
        }
        private void OnDestroy()
        {
            EnemyController.OnDoorDestroyed();
        }

        private void Death()
        {
            _navMesh.carving = false;
            EnemyController.Instance.RemoveDoor(gameObject);
            Destroy(gameObject); // si la puerta se puediera resconstruir este se cambia a un SetActive(false)
        }
    }
}

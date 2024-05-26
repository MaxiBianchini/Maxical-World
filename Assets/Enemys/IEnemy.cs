using UnityEngine;

namespace Enemys
{
    public interface IEnemy
    { 
        void Attack(GameObject currentTarget);
        void Death();
        void SetDestination(GameObject newDestination);
        void Initialize(float health, float damage, float speed, int value);
    }
}

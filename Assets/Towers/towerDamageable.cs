using Common;
using Enemys;
using UnityEngine;

namespace Towers
{
    public class TowerDamageable : MonoBehaviour, IDamageable
    {
        private float _health = 100;
        public void TakeDamage(float amount)
        {
            Debug.Log($"TOWER {gameObject.name} recibio  {amount} damage ");
            _health -= amount;
            if (_health <= 0)
            {
                Destroy();
            }
        }

        private void Destroy()
        {
            EnemyController.Instance.RemoveTower(gameObject);
            Destroy(gameObject);
        }
    }
}

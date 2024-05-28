using Common;
using UnityEngine;

namespace Nexo
{
    public class Nexo : MonoBehaviour, IDamageable
    {
        [SerializeField] private float health;
        public void TakeDamage(float amount)
        {
            Debug.Log($"NEXO {gameObject.name} recibio  {amount} damage");
            health -= amount;
            if (health <= 0)
            {
                Debug.Log("GAME OVER");
            }
        }
    }
}

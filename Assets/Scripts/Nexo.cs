using Common;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nexo
{
    public class Nexo : MonoBehaviour, IDamageable
    {
        [SerializeField] private float health;
        public void TakeDamage(float amount)
        {
           // Debug.Log($"NEXO {gameObject.name} recibio  {amount} damage");
            health -= amount;
            if (health <= 0)
            {
                GameOver();
                
            }
        }

        private void GameOver()
        {
            Debug.Log($"Game Over");
            SceneManager.LoadScene(1);
        }
    }
}

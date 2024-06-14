using System;
using Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Nexo
{


    public class Nexo : MonoBehaviour, IDamageable
    {
        [SerializeField] private bool testing = false;
        [SerializeField] private float maxHealth;
        
        private HealthBar healthBar;
        private float health;


        private void Start()
        {
            healthBar = GetComponentInChildren<HealthBar>();
            health = maxHealth;
            healthBar.SetMaxHealthValue(maxHealth);
            healthBar.UpdateHealthBar(health);
        }

        public void TakeDamage(float amount)
        {
           // Debug.Log($"NEXO {gameObject.name} recibio  {amount} damage");
            health -= amount;
            healthBar.UpdateHealthBar(health);
            if (health <= 0)
            {
                GameOver();
                
            }
        }

        private void GameOver()
        {
            Debug.Log($"Game Over");
            if (testing)
            {
                SceneManager.LoadScene(1);
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}

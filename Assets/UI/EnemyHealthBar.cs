using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] private Image healthBarImage;

        public void UpdateHealthBarA(float maxHealth, float currentHealth)
        {
            healthBarImage.fillAmount = currentHealth / maxHealth;
        }
    }
}

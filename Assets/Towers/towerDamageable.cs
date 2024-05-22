using Common;
using UnityEngine;

namespace Towers
{
    public class TowerDamageable : MonoBehaviour, IDamageable
    {
        // Start is called before the first frame update
        public void TakeDamage(float amount)
        {
            Debug.Log($"TOWER {gameObject.name} recibio  {amount} damage ");
        }
    }
}

using Common;
using UnityEngine;

namespace Nexo
{
    public class Nexo : MonoBehaviour, IDamageable
    {
        public void TakeDamage(float amount)
        {
            Debug.Log("NExo recibo damage " + amount);
        }
    }
}

using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    [SerializeField] private float damage = 10f;

    private BoxCollider boxCollider;
    private bool isAttacking =false;

    public bool IsAttacking { get { return isAttacking; } }

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamageable enemy = other.GetComponent<IDamageable>();
            if (enemy != null)
            {
                
                enemy.TakeDamage(damage);
            }
        }
        
    }

    private void DamageEventOn()
    {
        if (isAttacking) return;
        AudioManager.Instance.PlayEffect("Sword Slash");
        boxCollider.enabled = true;
        isAttacking = true;
    }

    private void DamageEventOff()
    {
        boxCollider.enabled = false;
        isAttacking = false;
    }
}

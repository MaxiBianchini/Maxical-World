using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHealthSystem : MonoBehaviour
{
    public event EventHandler onTowerHealthAmountMaxChanged;
    public event EventHandler onTowerDamaged;
    public event EventHandler onTowerHealed;
    public event EventHandler onTowerDied;

    private int healthAmount;

    [SerializeField] private int healthAmountMax;

    private void Awake()
    {
        healthAmount = healthAmountMax;
    }

    public void Damage(int damageAmount)
    {
        healthAmount -= damageAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, healthAmountMax);

        onTowerDamaged?.Invoke(this, EventArgs.Empty);

        if (isDead())
        {
            onTowerDied?.Invoke(this,EventArgs.Empty);
        }
    }

    private void Heal(int healAmount)
    {
        healthAmount += healAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, healthAmountMax);
        onTowerHealed?.Invoke(this, EventArgs.Empty);
    }

    public bool isDead()
    {
        return healthAmount <= 0;
    }

    public bool isFullHealth()
    {
        return healthAmount == healthAmountMax;
    }

    public int GetHealthAmount()
    {
        return healthAmount;
    }

    public int GetHealthAmountMax()
    {
        return healthAmountMax;
    }

    public float GetHealthAmountNormalized()
    {
        return (float) healthAmount / healthAmountMax;
    }

    public void SetHealthAmountMax(int healthAmountMax, bool updateHealthAmount)
    {
        this.healthAmountMax = healthAmountMax;

        if (updateHealthAmount)
        {
            healthAmount = healthAmountMax;
        }

        onTowerHealthAmountMaxChanged?.Invoke(this, EventArgs.Empty);
    }
    

}

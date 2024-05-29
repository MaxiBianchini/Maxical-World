using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Enemys;
using UnityEngine;

public class TowerHealthSystem : MonoBehaviour, IDamageable
{
    private BuildingTypeHolder buildingTypeHolder;

    //public event EventHandler onTowerHealthAmountMaxChanged;
    //public event EventHandler onTowerHealed;

    // public delegate void TowerDeathEventHandler(TowerHealthSystem tower);
    // public static event TowerDeathEventHandler OnTowerDeath;

    public event EventHandler onTowerDeath;
    public event EventHandler onTowerDamaged;


    private float currentHealth;
    private int maxHealth;

    //Test Variables
    public bool isDead;

    private void Awake()
    {
        buildingTypeHolder = GetComponent<BuildingTypeHolder>();
    }

    private void Start()
    {
        if (buildingTypeHolder != null)
        {
            maxHealth = buildingTypeHolder.buildingType.healthAmountMax;
            currentHealth = maxHealth;
        }
    }

    private void Update()
    {
        CheckHealth();
    }

    private void CheckHealth()
    {
        if (isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        // Notify suscriptors tower is dead
        if (onTowerDeath != null)
        {
            //OnTowerDeath(this); //Global event (not just for an instance)
            onTowerDeath?.Invoke(this, EventArgs.Empty);
            Debug.Log("La torre murio");
        }
        EnemyController.Instance.RemoveTower(gameObject);
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if(onTowerDamaged != null)
        {
            //Notify suscriptors tower has been damaged
            onTowerDamaged?.Invoke(this, EventArgs.Empty);
            Debug.Log("La torre recibio "+ damage + " de da�o");
        }
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }

    }

    //private void Heal(int healAmount)
    //{
    //    healthAmount += healAmount;
    //    healthAmount = Mathf.Clamp(healthAmount, 0, healthAmountMax);
    //    onTowerHealed?.Invoke(this, EventArgs.Empty);
    //}

    //public bool isDead()
    //{
    //    return healthAmount <= 0;
    //}

    //public bool isFullHealth()
    //{
    //    return healthAmount == healthAmountMax;
    //}

    //public int GetHealthAmount()
    //{
    //    return healthAmount;
    //}

    //public int GetHealthAmountMax()
    //{
    //    return healthAmountMax;
    //}

    //public float GetHealthAmountNormalized()
    //{
    //    return (float) healthAmount / healthAmountMax;
    //}

    //public void SetHealthAmountMax(int healthAmountMax, bool updateHealthAmount)
    //{
    //    this.healthAmountMax = healthAmountMax;

    //    if (updateHealthAmount)
    //    {
    //        healthAmount = healthAmountMax;
    //    }

    //    onTowerHealthAmountMaxChanged?.Invoke(this, EventArgs.Empty);
    //}
    

}

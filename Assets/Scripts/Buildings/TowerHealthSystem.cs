using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHealthSystem : MonoBehaviour
{
    private BuildingTypeHolder buildingTypeHolder;

    public event EventHandler onTowerHealthAmountMaxChanged;
    public event EventHandler onTowerHealed;

    public delegate void TowerDeathEventHandler(TowerHealthSystem tower);
    public static event TowerDeathEventHandler OnTowerDeath;

    public event EventHandler onTowerDamaged;


    private int currentHealth;
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
        //TO DO HACER CHECK DE SALUD
        CheckHealth();
    }

    private void CheckHealth()
    {
        //TO DO
        if (isDead)
        {
            // Llama al método para manejar la muerte de la torre
            Die();
        }
    }

    private void Die()
    {
        // Notify suscriptors tower is dead
        if (OnTowerDeath != null)
        {
            OnTowerDeath(this);
            Debug.Log("La torre murio");
        }

        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if(onTowerDamaged != null)
        {
            //Notify suscriptors tower has been damaged
            onTowerDamaged?.Invoke(this, EventArgs.Empty);
            Debug.Log("La torre recibio "+ damage + " de daño");
        }
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }

    }



    //private int healthAmount;

    //[SerializeField] private int healthAmountMax;

    //private void Awake()
    //{
    //    healthAmount = healthAmountMax;
    //}

    //public void Damage(int damageAmount)
    //{
    //    healthAmount -= damageAmount;
    //    healthAmount = Mathf.Clamp(healthAmount, 0, healthAmountMax);

    //    onTowerDamaged?.Invoke(this, EventArgs.Empty);

    //    if (isDead())
    //    {
    //        onTowerDied?.Invoke(this,EventArgs.Empty);
    //    }
    //}

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

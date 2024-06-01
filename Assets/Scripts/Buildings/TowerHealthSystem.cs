using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Enemys;
using UnityEngine;

public class TowerHealthSystem : MonoBehaviour, IDamageable
{
    [Tooltip("Indica el minimo para que la torre empiece a tirar fuego pequeño")]
    [SerializeField] private float smallFireDamageTrigger = 0.8f;
    [Tooltip("Indica el minimo para que la torre empiece a tirar fuego mediano")]
    [SerializeField] private float mediumFireDamageTrigger = 0.5f;
    [Tooltip("Indica el minimo para que la torre empiece a tirar fuego grande")]
    [SerializeField] private float bigFireDamageTrigger = 0.25f;
    private BuildingTypeHolder buildingTypeHolder;

    //public event EventHandler onTowerHealthAmountMaxChanged;
    //public event EventHandler onTowerHealed;

    // public delegate void TowerDeathEventHandler(TowerHealthSystem tower);
    // public static event TowerDeathEventHandler OnTowerDeath;

    public event EventHandler onTowerDeath;
    public event EventHandler<int> onTowerDamaged;

    public class OnTowerDamageEventArgs : EventArgs {

        public int towerHealth;

    }



    [SerializeField] private int maxHealth;

    public int MaxHealth { get { return maxHealth; } }

    [SerializeField] private int currentHealth;
    public int CurrentHealth { get { return currentHealth; } }

    private Dictionary<ParticleSystem, ParticleSystem> _instantiatedFireObjects = new Dictionary<ParticleSystem, ParticleSystem>();

    private ParticleSystem pfSmallFire;
    private ParticleSystem pfMediumFire;
    private ParticleSystem pfBigFire;

    private ParticleSystem smallFireVFX;
    private ParticleSystem mediumFireVFX;
    private ParticleSystem bigFireVFX;
    //Test Variables
    public bool isDead;

    private void Awake()
    {
        buildingTypeHolder = GetComponent<BuildingTypeHolder>();

        if (buildingTypeHolder != null)
        {
            maxHealth = buildingTypeHolder.buildingType.healthAmountMax;
            currentHealth = maxHealth;
        }
    }

    private void Start()
    {
        pfSmallFire = GameAssets.Instance.pfSmallFireVFX.GetComponent<ParticleSystem>();
        pfMediumFire = GameAssets.Instance.pfMediumFireVFX.GetComponent<ParticleSystem>();
        pfBigFire = GameAssets.Instance.pfBigFireVFX.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        CheckHealth();
    }

    private void CheckHealth()
    {
        PlayFireVFX();

        if (isDead)
        {
            Die();
        }
    }

    //Funcion inicial para ejecutar el fuego en las torres al ser herida
    private void PlayFireVFX()
    {
        float normalizeHealth = (float)currentHealth / maxHealth;
        int healthRange = 4;
        if (currentHealth <= 0)
        {
            healthRange = 0;

        }
        else if (normalizeHealth <= bigFireDamageTrigger)
        {
            healthRange = 1;

        }
        else if (normalizeHealth <= mediumFireDamageTrigger)
        {
            healthRange = 2;
        }
        else if (currentHealth <= smallFireDamageTrigger)
        {
            healthRange = 3;
        }


        UpdateVFX(pfSmallFire, healthRange == 3);
        UpdateVFX(pfMediumFire, healthRange == 2);
        UpdateVFX(pfBigFire, healthRange == 1);
    }

    //Funcion que determina que fuego tiene que ejecutar en la torre segun los parametros
    private void UpdateVFX(ParticleSystem fireObj, bool shouldPlay)
    {
        ParticleSystem vfx = null;
        if (_instantiatedFireObjects.ContainsKey(fireObj) && _instantiatedFireObjects[fireObj] != null)
        {
            vfx = _instantiatedFireObjects[fireObj];
        }
        else
        {
            vfx = Instantiate<ParticleSystem>(fireObj, transform.position + new Vector3(0f, 4f, 0f), Quaternion.identity, this.transform);
            _instantiatedFireObjects.Add(fireObj, vfx);
        }

        if (shouldPlay)
        {
            if (!vfx.isPlaying)
            {
                vfx.Play();
            }
            
        }
        else
        {
            vfx.Stop();
            
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

        GameObject explosion = GameAssets.Instance.pfBuildingDestroyedParticles.gameObject;
        GameObject explosionGO = Instantiate(explosion, transform.position + new Vector3(0f, 4f, 0f), Quaternion.identity);
        explosionGO.GetComponent<ParticleSystem>().Play();
        Destroy(explosionGO, 5f);
        EnemyController.Instance.RemoveTower(gameObject);
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= (int)damage;
        //Notify suscriptors tower has been damaged
        onTowerDamaged?.Invoke(this,  currentHealth);
        
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }

    }

 

}

using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;
using UnityEngine.UIElements;

public class ArrowProjectile : MonoBehaviour
{
    //public static method to create a bullet
    public static ArrowProjectile Create(Vector3 position, Transform enemy, float towerAccuracy)
    {
        Transform pfArrowProjectile  = GameAssets.Instance.pfArrowProjectile;
        Transform ArrowProjectileTransform = Instantiate(pfArrowProjectile,position,Quaternion.identity);

        ArrowProjectile arrowProjectile = ArrowProjectileTransform.GetComponent<ArrowProjectile>();
        arrowProjectile.SetAccuracy(towerAccuracy);
        arrowProjectile.SetTarget(enemy);
        return arrowProjectile;
    }

    private void SetAccuracy(float value)
    {
        accuracy = value;
    }

    [Header("Projectile Configuration")]
    //[SerializeField] float arrowMoveSpeed = 20f;
    [SerializeField] int arrowMinDamage = 10;
    [SerializeField] int arrowMaxDamage = 15;
    [Tooltip("Manage the duration of the bullet. Lower numbers make bullet go faster")]
    [SerializeField] float duration = 1f;
    [Tooltip("Manage the curve of the bullet path")]
    [SerializeField] private float heightY = 3f; //the height for the projectile
    [Tooltip("Type of curve")]
    [SerializeField] private AnimationCurve animCurve;

    [SerializeField] private Transform groundHitVFX;
    [SerializeField] private Transform enemyHitVFX;
    [SerializeField] private float xVFXRotation;

    //VFX Variables
    private Transform VFXParent;
    private Transform hitGroundVFX;

    //Position Variables
    private Transform targetEnemy;
    private Vector3 initialPosition;

    //bullet Accuracy
    private float accuracy;
    float hitChance;

    private void Start()
    {
        hitGroundVFX = GameAssets.Instance.pfHitGroundVFX;
        VFXParent = GameObject.FindGameObjectWithTag("InstancesGroupingObject").transform;
        this.transform.parent = VFXParent;
        initialPosition = transform.position;
        hitChance = UnityEngine.Random.Range(0f, 1f);
        
        StartCoroutine(ProjectilCurveRoutine(initialPosition,targetEnemy.transform.position));
    }

    //Manage the projectile to go from initial position to enemy position
    private IEnumerator ProjectilCurveRoutine(Vector3 startPosition, Vector3 endPosition)
    {
        float timePassed = 0f;
        float x = UnityEngine.Random.Range(1f, 3.5f);
        float z = UnityEngine.Random.Range(1f, 3.5f);

        while (timePassed < duration)
        {
            if (targetEnemy)
            {
                if (hitChance <= accuracy)
                {
                    endPosition = targetEnemy.transform.position;
                }
                else
                {
                    endPosition = targetEnemy.transform.position + new Vector3(x, -5f, z);
                }
                
            }
            


            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;
            float heightT = animCurve.Evaluate(linearT); //this evaluates in each linearT moment the curve
            float height = Mathf.Lerp(0f, heightY, heightT);

            transform.position = Vector3.Lerp(startPosition, endPosition, linearT) + new Vector3(0f, height);
            RotateProjectileDirection(heightT);

            yield return null;
        }

    }

    //Set enemy target
    private void SetTarget(Transform enemy)
    {
        this.targetEnemy = enemy;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable enemy = other.GetComponent<IDamageable>();
        if (enemy != null)
        {
            if (hitChance <= accuracy)
            {
                //Damage the enemy
                float damage = UnityEngine.Random.Range(arrowMinDamage, arrowMaxDamage);
                //Debug.Log("Enemy get " + damage + " of damage");
                enemy.TakeDamage(damage);
                
                Transform hitVFX = Instantiate(enemyHitVFX, this.transform.position, Quaternion.identity);
                hitVFX.eulerAngles = new Vector3(xVFXRotation, 0f, 0f);
                hitVFX.GetComponent<ParticleSystem>().Play();
                hitVFX.transform.parent = VFXParent;
            }
        }
        else
        {
            //Missed and instantiate particles on the object hit

           Transform hitVFX = Instantiate(groundHitVFX, this.transform.position, Quaternion.identity);
            hitVFX.eulerAngles = new Vector3(-90f, 0f, 0f);
            hitVFX.GetComponent<ParticleSystem>().Play();
            hitVFX.transform.parent = VFXParent;
            
            //Destroy(hitVFX, 2f);

        }
        
        


        Destroy(gameObject);
    }

    //Apunto la flecha a la direccion del enemigo.
    private void RotateProjectileDirection(float curve)
    {
        if (!targetEnemy) return;

        Vector3 direction = targetEnemy.transform.position - transform.position;
        Vector3 xDirection = new Vector3(-curve, transform.position.y, direction.z);
        Quaternion lookRotation = Quaternion.LookRotation(xDirection);
        transform.rotation = lookRotation;
    }





}

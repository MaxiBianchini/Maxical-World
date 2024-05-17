using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ArrowProjectile : MonoBehaviour
{
    public static ArrowProjectile Create(Vector3 position, EnemyTestLeo enemy)
    {
        Transform pfArrowProjectile  = GameAssets.Instance.pfArrowProjectile;
        Transform ArrowProjectileTransform = Instantiate(pfArrowProjectile,position,Quaternion.identity);

        ArrowProjectile arrowProjectile = ArrowProjectileTransform.GetComponent<ArrowProjectile>();
        arrowProjectile.SetTarget(enemy);
        return arrowProjectile;
    }
    
    [SerializeField] float arrowMoveSpeed = 20f;
    [SerializeField] float duration = 1f;
    [SerializeField] private float heightY = 3f; //the height for the projectile
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float accuracy = .75f;
    private EnemyTestLeo targetEnemy;

    private Vector3 initialPosition;
    private Vector3 lastMoveDir;
    private float timeToDie = 2f;

    private void Start()
    {
        initialPosition = transform.position;
        StartCoroutine(ProjectilCurveRoutine(initialPosition,targetEnemy.transform.position));
    }

    private void Update()
    {
        //Vector3 moveDir;
        //if(targetEnemy != null)
        //{
        //    moveDir = (targetEnemy.transform.position - transform.position).normalized;
        //    lastMoveDir = moveDir;
        //}
        //else
        //{
        //    moveDir = lastMoveDir;
        //}

        ////transform.eulerAngles = new Vector3 (0,)
        //transform.position += moveDir * arrowMoveSpeed * Time.deltaTime;

        //timeToDie -= Time.deltaTime;
        //if(timeToDie <= 0)
        //{
        //    Destroy(gameObject);
        //}
    }

    private IEnumerator ProjectilCurveRoutine(Vector3 startPosition, Vector3 endPosition)
    {
        float timePassed = 0f;

        while (timePassed < duration)
        {
            endPosition = targetEnemy.transform.position;
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;
            float heightT = animCurve.Evaluate(linearT); //this evaluates in each linearT moment the curve
            float height = Mathf.Lerp(0f, heightY, heightT);

            //The last + new Vector adds the curve for the projectile
            transform.position = Vector3.Lerp(startPosition, endPosition, linearT) + new Vector3(0f, height);
            
            yield return null;
        }

        Destroy(gameObject);
    }
    private void SetTarget(EnemyTestLeo enemy)
    {
        this.targetEnemy = enemy;
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyTestLeo enemy = other.GetComponent<EnemyTestLeo>();
        if(enemy != null)
        {
            
            float hitChance = UnityEngine.Random.Range(0f, 1f);
            
            if(hitChance <= accuracy)
            {
                Debug.Log("Hit");
                int damageAmount = 10;
                //DAMAGE ENEMY
                //enemy.GetComponent<TowerHealthSystem>().Damage(damageAmount);
            }
            else
            {
                //Instanciar particulas de polvo en el piso
                Debug.Log("Missed");
            }
            //Hit an enemy
            Destroy(gameObject);
        }
    }





}

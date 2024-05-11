using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ArrowProjectile : MonoBehaviour
{
    public static ArrowProjectile Create(Vector3 position, Enemy enemy)
    {
        Transform pfArrowProjectile  = GameAssets.Instance.pfArrowProjectile;
        Transform ArrowProjectileTransform = Instantiate(pfArrowProjectile,position,Quaternion.identity);

        ArrowProjectile arrowProjectile = ArrowProjectileTransform.GetComponent<ArrowProjectile>();
        arrowProjectile.SetTarget(enemy);
        return arrowProjectile;

    }
    
    [SerializeField] float arrowMoveSpeed = 20f;
    private Enemy targetEnemy;

    private Vector3 lastMoveDir;
    private float timeToDie = 2f;


    private void Update()
    {
        Vector3 moveDir;
        if(targetEnemy != null)
        {
            moveDir = (targetEnemy.transform.position - transform.position).normalized;
            lastMoveDir = moveDir;
        }
        else
        {
            moveDir = lastMoveDir;
        }

        //transform.eulerAngles = new Vector3 (0,)
        transform.position += moveDir * arrowMoveSpeed * Time.deltaTime;

        timeToDie -= Time.deltaTime;
        if(timeToDie <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void SetTarget(Enemy enemy)
    {
        this.targetEnemy = enemy;
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if(enemy != null)
        {
            //Hit an enemy
            int damageAmount = 10;
            enemy.GetComponent<TowerHealthSystem>().Damage(damageAmount);
            Destroy(gameObject);
        }
    }





}

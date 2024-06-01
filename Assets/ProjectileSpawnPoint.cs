using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProjectileSpawnPoint : MonoBehaviour
{
    private Coroutine coroutine;

    private void Update()
    {
        Vector3 startPoint = transform.position;
        Vector3 endPoint = transform.position + transform.forward * 5.0f;

        Debug.DrawLine(startPoint, endPoint, Color.yellow);
        
    }

    

    public void RotateTowardsEnemy(Transform enemy)
    {
       if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        
        coroutine =  StartCoroutine(StartRotation(enemy));
        
    }

    private IEnumerator StartRotation(Transform enemy)
    {

        while (enemy)
        {
                Vector3 rotateDirection = enemy.transform.position - transform.position;
                Quaternion rotation = Quaternion.LookRotation(rotateDirection);
                transform.rotation = rotation;

                yield return new WaitForSeconds(.1f);
        }

        
    }
}

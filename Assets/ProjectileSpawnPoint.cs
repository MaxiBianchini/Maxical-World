using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProjectileSpawnPoint : MonoBehaviour
{
    
    private Coroutine coroutine;

    public event EventHandler<Quaternion> onRotate;

 

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
            Vector3 direction = enemy.transform.position - transform.position;
            //Vector3 rotateDirection = new Vector3(transform.rotation.x, direction.y, transform.rotation.z);
            //direction.x = 0f;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = rotation;
            onRotate?.Invoke(this, rotation);

            yield return new WaitForSeconds(.1f);
        }

        
    }
}

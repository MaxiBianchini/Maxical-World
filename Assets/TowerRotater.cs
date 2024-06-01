using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TowerRotater : MonoBehaviour
{
    private ProjectileSpawnPoint spawnPoint;
    private Coroutine coroutine;
    private void Awake()
    {
        spawnPoint = GetComponentInParent<ProjectileSpawnPoint>();
    }

    public void RotateTowardsEnemy(Transform enemy)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(StartRotation(enemy));

    }

    private IEnumerator StartRotation(Transform enemy)
    {

        while (enemy)
        {
            Vector3 direction = enemy.transform.position - transform.position;
            //Vector3 rotateDirection = new Vector3(transform.rotation.x, direction.y, transform.rotation.z);
            //direction.x = 0f;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(0f, rotation.eulerAngles.y,0f);

            yield return new WaitForSeconds(.1f);
        }


    }
}

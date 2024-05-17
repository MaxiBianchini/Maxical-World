using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private float destroyTime = 3f;
    [SerializeField] private Vector3 offset = new Vector3(0, 3, 0);
    [SerializeField] private Vector3 randomizeIntensity = new Vector3(0.5f, 0.5f, 0.5f);
    void Start()
    {
        Destroy(gameObject, destroyTime);
        transform.localPosition += offset;
        float x = Random.Range(-randomizeIntensity.x, randomizeIntensity.x);
        float y = Random.Range(-randomizeIntensity.y, randomizeIntensity.y);
        float z = Random.Range(-randomizeIntensity.z, randomizeIntensity.z);
        transform.localPosition += new Vector3(x,y,z);
    }

  
}

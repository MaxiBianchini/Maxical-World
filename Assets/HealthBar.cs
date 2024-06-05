using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Camera mainCamera;
    private Slider healthBar;


    private void Awake()
    {
        healthBar = GetComponent<Slider>();
    }
    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (mainCamera != null)
        {
            Vector3 direction = (mainCamera.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = lookRotation;
        }
    }
    
    public void SetMaxHealthValue(int value)
    {
        healthBar.maxValue = value;
    }

    public void UpdateHealthBar(int value)
    {
        healthBar.value = value;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIHealth : MonoBehaviour
{
    private TowerHealthSystem towerHealthSystem;
    private Slider healthBar;

    private void Awake()
    {
        towerHealthSystem = GetComponentInParent<TowerHealthSystem>();
        healthBar = GetComponentInChildren<Slider>();
    }
    private void Start()
    {
        healthBar.maxValue = towerHealthSystem.MaxHealth;
        healthBar.value = towerHealthSystem.CurrentHealth;
    }
    private void OnEnable()
    {
        //Debug.Log($"Me suscribo {towerHealthSystem}");

        if (towerHealthSystem != null)
        {
            towerHealthSystem.onTowerDamaged += ChangeHealthBar;
        }
    }

    private void OnDisable()
    {
        if (towerHealthSystem != null)
        {
            towerHealthSystem.onTowerDamaged -= ChangeHealthBar;
            Debug.Log("Me desuscribo");
        }
    }


    public void ChangeHealthBar(object sender, int value)
    {
        if (towerHealthSystem != null)
        {

            healthBar.value = value;

        }
    }
}

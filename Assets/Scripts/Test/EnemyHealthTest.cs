using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthTest : MonoBehaviour
{
    //[SerializeField] int currentHealth = 100;
    [SerializeField] private GameObject pfFloatingText;
    public void GetDamage(int value)
    {
        if(pfFloatingText)
        {
            ShowFloatingText(value);
        }
    }

    private void ShowFloatingText(int value)
    {
        var textObject = Instantiate(pfFloatingText, transform.position, Quaternion.identity, transform);
        textObject.GetComponent<TextMesh>().text = value.ToString(); 
    }
}

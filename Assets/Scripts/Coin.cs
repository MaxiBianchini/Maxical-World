using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private float _value;

    public void Initialize(float value)
    {
        _value = value;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CoinManager.Instance.AddCoinMount(_value);
            Destroy();
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}

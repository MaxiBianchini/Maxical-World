using System;
using Common;
using Enemys;
using UnityEngine;

namespace Towers
{
    public class TowerDamageable : MonoBehaviour
    {
        [SerializeField] private GameObject tower;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                tower.GetComponent<TowerHealthSystem>().Die();
            }
        }
    }
}

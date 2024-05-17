using System;
using UnityEngine;
using UnityEngine.AI;

namespace Enemys.Chaser
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class ChaserMovement : MonoBehaviour
    {
        public Vector3 Destination { get; }
        public float Health { get; set; }
        public float Damage { get; }
        public float AttackRange { get; }
        public float AttackSpeed { get; }

        private NavMeshAgent _agent;

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            throw new NotImplementedException();
        }


        public void TakeDamage(float amount)
        {
            throw new System.NotImplementedException();
        }

        public void Attack()
        {
            throw new System.NotImplementedException();
        }

        public void Death()
        {
            throw new NotImplementedException();
        }

        public void SetDestination(Vector3 newDestination)
        {
            throw new NotImplementedException();
        }
        
    }
}
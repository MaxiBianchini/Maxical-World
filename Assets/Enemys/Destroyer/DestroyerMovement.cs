using System;
using System.Collections;
using Common;
using Doors;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Enemys.Destroyer
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class DestroyerMovement : MonoBehaviour, IEnemy, IDamageable
    {
        [SerializeField] private float health;
        [SerializeField] private float damage;
        [SerializeField] private float attackRange;
        [SerializeField] private float attackSpeed;

        private NavMeshAgent _agent;
        private Coroutine _attackCoroutine;
        private bool _isAttacking = false;
        private Vector3 _destination;

        private GameObject _target;
        
        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            AttackPerform();
        }
        
        public void TakeDamage(float amount)
        {
            Debug.Log("take damage " + amount);
            health -= amount;
            if (health <= 0)
            {
                Death();
            }
        }

        public void Attack(GameObject currentTarget)
        {
            Debug.Log("attack! " + currentTarget.name);
            IDamageable damageable = currentTarget.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(10);
            }
            else
            {
                Debug.LogError("Attack() - No se le puede hacer danio: " + currentTarget.name);
            }
            
        }

        public void Death()
        {
            Debug.Log("mori");
        }

        public void SetDestination(GameObject newDestination)
        {
            _target = newDestination;
            Debug.Log("Target: " + _target.tag);
            
            _destination = newDestination.transform.position;
            _agent.SetDestination(_destination);
        }

        private void AttackPerform()
        {
            float distanceToDestination = Vector3.Distance(transform.position, _destination);
            
            if (distanceToDestination <= attackRange && !_isAttacking)
            {
                _agent.isStopped = true;
                _isAttacking = true;
                StartAttacking();
            }
            else if (distanceToDestination > attackRange)
            {
                _agent.isStopped = false;
                _isAttacking = false;
                StopAttacking();
            }
        }

        private void StartAttacking()
        {
            if (_attackCoroutine == null)
            {
                _attackCoroutine = StartCoroutine(AttackDelay());
            }
        }

        private void StopAttacking()
        {
            if (_attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine);
                _attackCoroutine = null;
            }
        }
        
        private IEnumerator AttackDelay()
        {
            while (_isAttacking)
            {
                yield return new WaitForSeconds(10/attackSpeed);
                Attack(_target);
            }
        }
    }
}

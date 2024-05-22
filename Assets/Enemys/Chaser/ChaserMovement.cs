using System;
using System.Collections;
using Common;
using Enemys.Data;
using Player.Scripts;
using UnityEngine;
using UnityEngine.AI;

namespace Enemys.Chaser
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class ChaserMovement : MonoBehaviour, IEnemy, IDamageable
    {
        [SerializeField] private float attackRange;
        [SerializeField] private float attackSpeed;
        [SerializeField] private float rotationSpeed;

        private float _damage;
        private float _health;
        private bool _isAttacking = false;
        private NavMeshAgent _agent;
        private Coroutine _attackCoroutine;
        private Vector3 _destination;
        private Vector3 _direction;
        private GameObject _target;
        private Quaternion _targetRotation;
        private IDamageable _damageable;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            StopAttacking();
        }

        private void Update()
        {
            CheckRange();
            Chase();

        }

        public void Initialize(float health, float damage, float speed)
        {
            _health = health;
            _damage = damage;
            _agent.speed = speed;
        }
        
        private void Chase()
        {
            Debug.Log($"Is attacking chase  {_isAttacking} to {_target}");
            if (_target != null && !_isAttacking)
            {
                _agent.SetDestination(_target.transform.position);
            }
        }
        public void TakeDamage(float amount)
        {
            _health -= amount;
            if (_health <= 0)
            {
                Death();
            }
        }

        public void Attack(GameObject currentTarget)
        {
            if (_damageable != null)
            {
                _damageable.TakeDamage(_damage);
            }
            else
            {
                Debug.LogError("Attack() - No se le puede hacer danio: " + currentTarget.name + " - Puede faltar componente IDamageable");
            }
        }

        public void Death()
        {
            //EnemyController.Instance.RemoveChaserFromList(gameObject);
            StopAttacking();
            Destroy(gameObject);
        }

        public void SetDestination(GameObject newDestination)
        {
            _target = newDestination;
            _damageable = _target.GetComponent<IDamageable>();
            if (_target != null)
            {
                _destination = _target.transform.position;
                _agent.SetDestination(_destination);
            }
        }

        private void StartAttacking()
        {
            if (_attackCoroutine == null)
            {
                _attackCoroutine = StartCoroutine(AttackPerform());
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

        private void CheckRange()
        {
            float distanceToDestination = Vector3.Distance(transform.position, _target.transform.position);
            
            if (distanceToDestination <= attackRange && !_isAttacking)
            {
                _isAttacking = true;
                _agent.isStopped = true;
                StartAttacking();
            }
            
            else if (distanceToDestination > attackRange && _isAttacking)
            {
                _isAttacking = false;
                _agent.isStopped = false;
                StopAttacking();
            }
            if (_isAttacking) //mira al obeetivo
            {
                _direction = _target.transform.position - transform.position;
                _targetRotation = Quaternion.LookRotation(_direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime * rotationSpeed);
            }
        }
        
        private IEnumerator AttackPerform()
        {
            while (_isAttacking)
            {
                yield return new WaitForSeconds(10/attackSpeed);
                Attack(_target);
              //  Debug.Log($"Attack {_target}");
            }
        }
        
    }
}
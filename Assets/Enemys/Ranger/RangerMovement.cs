using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;
using UnityEngine.AI;

namespace Enemys.Ranger
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class RangerMovement : MonoBehaviour, IDamageable, IEnemy
    {
        [SerializeField] private float attackRange;
        [SerializeField] private float attackSpeed;
        
        [Header("Projectile")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float bulletSpeed;


        private float _damage;
        private float _health;
        private bool _isAttacking = false;
        private NavMeshAgent _agent;
        private Coroutine _attackCoroutine;
        private Vector3 _destination;
        private GameObject _target;
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
        }

        public void Initialize(float health, float damage, float speed)
        {
            _health = health;
            _damage = damage;
            _agent.speed = speed;
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
                //_damageable.TakeDamage(_damage);
                GameObject projectile = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                Bullet shootBullet = projectile.GetComponent<Bullet>();
                shootBullet.Initialize(_damage, bulletSpeed, _damageable, currentTarget.transform.position);
            }
            else
            {
                Debug.LogError("Attack() - No se le puede hacer danio: " + currentTarget.name + " - Puede faltar componente IDamageable");
            }
        }

        public void Death()
        {
            //EnemyController.Instance.RemoveRangerFromList(gameObject);
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
            float distanceToDestination = Vector3.Distance(transform.position, _destination);
            
            if (distanceToDestination <= attackRange && !_isAttacking)
            {
                _isAttacking = true;
                _agent.isStopped = true;
                StartAttacking();
            }
            
            else if (distanceToDestination > attackRange)
            {
                _isAttacking = false;
                _agent.isStopped = false;
                StopAttacking();
            }
        }
        
        private IEnumerator AttackPerform()
        {
            while (_isAttacking)
            {
                yield return new WaitForSeconds(10/attackSpeed);
                Attack(_target);
                Debug.Log($"Attack {_target}");
            }
        }
        
    }
}
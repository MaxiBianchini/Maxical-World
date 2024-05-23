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
        [SerializeField] private float rotationSpeed;
        
        [Header("Projectile")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float bulletSpeed;


        private float _damage;
        private float _health;
        private int _value;
        private bool _isAttacking = false;
        private NavMeshAgent _agent;
        private Coroutine _attackCoroutine;
        private Vector3 _destination;
        private Vector3 _direction;
        private GameObject _target;
        private Bullet _shootBullet;
        private IDamageable _damageable;
        private Quaternion _targetRotation;
        
        private EnemyController _enemyController;
        private float _distance;

        
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }
        
        private void Start()
        {
            StopAttacking();
            _enemyController = EnemyController.Instance;

        }
        private void Update()
        {
            CheckRangeAndAttack();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Death();
            }
        }

        public void Initialize(float health, float damage, float speed, int value)
        {
            _health = health;
            _damage = damage;
            _agent.speed = speed;
            _value = value;
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
            if (_damageable != null && currentTarget != null)
            {
                //_damageable.TakeDamage(_damage);
                GameObject projectile = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                _shootBullet = projectile.GetComponent<Bullet>();
                _shootBullet.Initialize(_damage, bulletSpeed, _damageable, currentTarget.transform.position);
            }
            else if (currentTarget == null)
            {
                ChangeTarget();
            }
            else
            {
                Debug.LogError("Attack() - No se le puede hacer danio: " + currentTarget.name + " - Puede faltar componente IDamageable");
            }
        }

        public void Death()
        {
            StopAttacking();
            EnemyController.Instance.DropCoin(gameObject.transform, _value);
            EnemyController.Instance.enemiesList.Remove(gameObject);
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
        
        private void ChangeTarget()
        {
            int towerCount = _enemyController.Towers.Count;
            int doorCount = _enemyController.Doors.Count;
            GameObject closestGameObject = null;
            float closestDistance = Mathf.Infinity;
            float currentDistance;
            if (towerCount != 0)
            {
                foreach (var tower in EnemyController.Instance.Towers)
                {
                    currentDistance = Vector3.Distance(gameObject.transform.position, tower.transform.position);
                    if (currentDistance < closestDistance)
                    {
                        closestDistance = currentDistance;
                        closestGameObject = tower;
                    }
                }
                SetDestination(closestGameObject);
            }
            else if (doorCount != 0)
            {
                foreach (var door in EnemyController.Instance.Doors)
                {
                    currentDistance = Vector3.Distance(gameObject.transform.position, door.transform.position);
                    if (currentDistance < closestDistance)
                    {
                        closestDistance = currentDistance;
                        closestGameObject = door;
                    }
                }
                SetDestination(closestGameObject); 
            }
            else
            {
                closestGameObject = EnemyController.Instance.Nexo;
                SetDestination(closestGameObject);
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
        
        private void CheckRangeAndAttack()
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
            LookTarget();
        }
        private void LookTarget()
        {
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
                Debug.Log($"Attack {_target}");
            }
        }
    }
}
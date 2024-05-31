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
        [SerializeField] private LayerMask damageable;
        
        [Header("Projectile")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float bulletSpeed;


        private float _damage;
        private float _health;
        private int _value;
       // private bool _isAttacking = false;
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

        private State _state;
        
        private enum State
        {
            Chasing,
            Attacking,
        }
        
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
            Behaviour();
            LookTarget();

        }

        public void Initialize(float health, float damage, float speed, int value)
        {
            _health = health;
            _damage = damage;
            _agent.speed = speed;
            _value = value;
            _state = State.Chasing;
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
            LookTarget();
            if (_damageable != null && currentTarget != null)
            {
                GameObject projectile = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                _shootBullet = projectile.GetComponent<Bullet>();
                _shootBullet.Initialize(_damage, bulletSpeed, _damageable, currentTarget.transform.position);
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
            else
            {
                ChangeTarget();
            }
        }

        private void Behaviour() //checkeando constantemente
        {

            bool isTargetVisible = IsTargetVisible();
            bool isInRange = IsInRange();
            Debug.Log($"State: {_state}, IsTargetVisible: {isTargetVisible}, IsInRange: {isInRange}");
            switch (_state)
            {
                case State.Chasing:
                    if (isTargetVisible && isInRange)
                    {
                        Debug.Log($"Chasing to Attacking");

                        _state = State.Attacking;
                        StartAttacking();
                       
                    }
                    break;
                case State.Attacking:
                    if (_target != null) 
                    {
                        if (!isTargetVisible || !isInRange)
                        {
                            Debug.Log($"Attacking to Chasing");
                            _state = State.Chasing;
                            StopAttacking();
                            SetDestination(_target);
                        }
                        
                    }
                    else
                    {
                        ChangeTarget();
                    }
                    break;
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
            Debug.Log($"Cambiando target a {closestGameObject}");
        }
        
        private void StartAttacking()
        {
            if (_attackCoroutine == null)
            {
                _agent.isStopped = true;
                _attackCoroutine = StartCoroutine(AttackDelay());
            }
        }

        private void StopAttacking()
        {
            if (_attackCoroutine != null)
            {
                _agent.isStopped = false;
                StopCoroutine(_attackCoroutine);
                _attackCoroutine = null;
            }
        }
        
        private bool IsInRange()
        {
            float distanceToDestination = Vector3.Distance(transform.position, _destination);

            return distanceToDestination <= attackRange;
        }
        private void LookTarget()
        {
            if (_state == State.Attacking && _target != null) //mira al obeetivo
            {
                _direction = _target.transform.position - transform.position;
                _targetRotation = Quaternion.LookRotation(_direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime * rotationSpeed);
            }
        }
        
        private bool IsTargetVisible()
        {
            if (_target == null)
            {
                Debug.LogWarning("IsTargetVisible() - target nulo");
                return false;
            }
            Debug.Log($"Target: {_target}");
    
            Vector3 directionToTarget = _target.transform.position - transform.position;

            RaycastHit hit;
            
            if (Physics.Raycast(transform.position, directionToTarget, out hit, attackRange, damageable))
            {
                Debug.DrawRay(transform.position, hit.transform.position - transform.position, Color.red); //todo borrar cuando ande todo
                Debug.Log($"Raycast hit: {hit.transform.name}");
                if (hit.transform == _target.transform)
                {
                    return true;
                }
            }
            return false;
        }
        
        
        
        private IEnumerator AttackDelay()
        {
            while (_state == State.Attacking)
            {
                Attack(_target);
                yield return new WaitForSeconds(10/attackSpeed);
                //Debug.Log($"Attack {_target}");
            }
        }
    }
}
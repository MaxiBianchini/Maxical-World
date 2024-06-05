using System;
using System.Collections;
using Common;
using Doors;
using UI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Enemys.Destroyer
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class DestroyerMovement : MonoBehaviour, IEnemy, IDamageable
    {
        [SerializeField] private float attackRange;
        [SerializeField] private float attackSpeed;
        [SerializeField] private EnemyHealthBar healthBar;

        private float _damage;
        private float _health;
        private int _value;
        private NavMeshAgent _agent;
        private Coroutine _attackCoroutine;
        private bool _isAttacking = false;
        private Vector3 _destination;
        private GameObject _target;
        private IDamageable _damageable;
        private float _maxHealth;
        private bool _isDead = false;

        private AnimationsController _animationsController;
        
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animationsController = GetComponent<AnimationsController>();
        }

        private void Start()
        {
            StopAttacking();
            healthBar.UpdateHealthBar(_maxHealth, _health);
            _isDead = false;
        }

        private void Update()
        {
            CheckRange();
        }
       

        public void Initialize(float health, float damage, float speed, int value)
        {
            _health = health;
            _maxHealth = _health;
            _damage = damage;
            _agent.speed = speed;
            _value = value;
        }

        public void TakeDamage(float amount)
        {
            _health -= amount;
            _animationsController.Hit();
            healthBar.UpdateHealthBar(_maxHealth, _health);
            if (_health <= 0 && !_isDead)
            {
                Death();
            }
        }

        public void Attack(GameObject currentTarget)
        {
            if (_damageable != null)
            {
                _animationsController.SetMovingState(false);
                _animationsController.Attack();
                _damageable.TakeDamage(_damage);
            }
            else
            {
                Debug.LogError("Attack() - No se le puede hacer danio: " + currentTarget.name + " - Puede faltar componente IDamageable");
            }
        }

        public void Death()
        {
            Debug.Log("Death method called");
            StopAttacking();
            _isDead = true;
            _animationsController.SetDead();
            CoinManager.Instance.DropCoin(gameObject.transform, _value);
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
                _animationsController.SetMovingState(true);
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
              //  Debug.Log($"Attack {_target}");
            }
        }
    }
}

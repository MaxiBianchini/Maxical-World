using System.Collections;
using Common;
using Enemys;
using Enemys.Ranger;
using UI;
using UnityEngine;
using UnityEngine.AI;

namespace Ranger
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class RangerMovement : MonoBehaviour, IDamageable, IEnemy
    {
        [SerializeField] private float attackRange;
        [SerializeField] private float attackSpeed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private LayerMask validTarget;
        
        [Header("Projectile")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float bulletSpeed;

        [SerializeField] private bool isInRange;
        [SerializeField] private bool isVisible;
        
        private HealthBar _healthBar;
        private float _damage;
        private float _health;
        private int _value;
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
        private float _maxHealth;
        private bool _isDead = false;
        
        private AnimationsController _animationsController;
        
        private enum State
        {
            Chasing,
            Attacking,
        }
        
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animationsController = GetComponent<AnimationsController>();
            _healthBar = GetComponentInChildren<HealthBar>();
        }
        
        private void Start()
        {
            StopAttacking();
            _enemyController = EnemyController.Instance;
            _maxHealth = _health;
            GetComponentInChildren<HealthBar>().SetMaxHealthValue(_maxHealth);
            _healthBar.UpdateHealthBar(_health);
            _isDead = false;

        }
        private void Update()
        {
            Behaviour();
            if (_agent.isStopped)
            {
                _animationsController.SetMovingState(false);
            }
            else
            {
                _animationsController.SetMovingState(true);
            }

            isInRange = IsInRange();
            isVisible = IsTargetVisible();
        }

        public void Initialize(float health, float damage, float speed, int value)
        {
            _health = health;
            _maxHealth = _health;
            _damage = damage;
            _agent.speed = speed;
            _value = value;
            _state = State.Chasing;
        }

        public void TakeDamage(float amount)
        {
            _health -= amount;
            _animationsController.Hit();
            _healthBar.UpdateHealthBar(_health);
            if (_health <= 0 && !_isDead)
            {
                Death();
            }
        }

        public void Attack(GameObject currentTarget)
        {
            if (_damageable != null && currentTarget != null)
            {
                _animationsController.SetMovingState(false);
                _animationsController.Attack();
                GameObject projectile = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                _shootBullet = projectile.GetComponent<Bullet>();
                _shootBullet.Initialize(_damage, bulletSpeed, _damageable, currentTarget.transform.position);
            }
            
            else
            {
                Debug.LogError("Attack() - No se le puede hacer danio: " + currentTarget.name + " - Puede faltar componente IDamageable");
            }
            if (currentTarget == null)
            {
                StopAttacking();
                ChangeTarget();
            }
        }

        public void Death()
        {
            StopAttacking();
            _isDead = true;
            _animationsController.SetDead();
            AudioManager.Instance.PlayEffect("Enemy Death");
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
                _agent.isStopped = false;
                _agent.SetDestination(_destination);
            }
            else
            {
                ChangeTarget();
            }
        }

        private void Behaviour() //checkeando constantemente
        {
            if (_target == null)
            {
                ChangeTarget();
            }
            else
            {
                //  Debug.Log($"State: {_state}, IsTargetVisible: {IsTargetVisible()}, IsInRange: {IsInRange()}");
                switch (_state)
                {
                    case State.Chasing:
                        
                        if (IsTargetVisible() && IsInRange())
                        {
                            // Debug.Log($"Chasing to Attacking");
                            _state = State.Attacking;
                            StartAttacking();
                       
                        }
                        else
                        {
                            SetDestination(_target);
                        }
                        break;
                    case State.Attacking:
                        LookTarget();
                        if (_target != null) 
                        {
                            if (!IsTargetVisible() || !IsInRange())
                            {
                                // Debug.Log($"Attacking to Chasing");
                                _state = State.Chasing;
                                StopAttacking();
                                SetDestination(_target);
                            }
                        }
                        else
                        {
                            StopAttacking();
                            _agent.isStopped = true;
                            _state = State.Chasing;
                            ChangeTarget();
                        }
                        break;
                }
            }
            
        }
        //todo borrar si funciona bien
        private void OnDrawGizmos()
        {
            if (_target != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(_destination, 0.5f); // Dibuja una esfera roja en el destino
            }
        
            if (_agent != null && _agent.hasPath)
            {
                Gizmos.color = Color.yellow;
                Vector3[] path = _agent.path.corners;
        
                for (int i = 0; i < path.Length - 1; i++)
                {
                    Gizmos.DrawLine(path[i], path[i + 1]); // Dibuja líneas rojas para la trayectoria
                }
            }
            
        }
        
        private void ChangeTarget()
        {
            Debug.LogWarning($"Target {_target}");
            _target = null;
            _agent.isStopped = true;
            int towerCount = EnemyController.Instance.Towers.Count;
            int doorCount = EnemyController.Instance.Doors.Count;
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
            if (closestGameObject == null)
            {
                Debug.LogError("NULO");
            }
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
            if (_target == null)
            {
                return false;
            }
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
    
            Vector3 directionToTarget = _target.transform.position - transform.position;

            RaycastHit hit;
            
            if (Physics.Raycast(transform.position, directionToTarget, out hit, attackRange, validTarget))
            {
                Debug.DrawRay(transform.position, hit.transform.position - transform.position, Color.red); //todo borrar cuando ande todo
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
            }
        }
    }
}
using System.Collections;
using Common;
using Enemys;
using UI;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ChaserMovement : MonoBehaviour, IEnemy, IDamageable
{
    [SerializeField] private float attackRange;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float rotationSpeed;
    
    
    private HealthBar _healthBar;
    private float _damage;
    private float _health;
    private float _maxHealth;
    private int _value;
    private NavMeshAgent _agent;
    private Coroutine _attackCoroutine;
    private Vector3 _destination;
    private Vector3 _direction;
    private GameObject _target;
    private Quaternion _targetRotation;
    private IDamageable _damageable;
    private EnemyController _enemyController;
    private bool _isDead = false;
    private State _state;
        
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
            _maxHealth = _health;
            GetComponentInChildren<HealthBar>().SetMaxHealthValue((int)_maxHealth);
            _healthBar.UpdateHealthBar(_health);
            _state = State.Chasing;
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

    }

    public void Initialize(float health, float damage, float speed, int value)
    {
        _health = health;
        _maxHealth = _health;
        _damage = damage;
        _agent.speed = speed;
        _value = value;
            
    }
    private void Behaviour() //checkeando constantemente
    {
        Chase();
        if (_target == null)
        {
            ChangeTarget();
        }
        else
        {
            switch (_state)
            {
                case State.Chasing:
                    
                    if (IsInRange())
                    {
                        //_animationsController.SetMovingState(false);
                        _state = State.Attacking;
                        StartAttacking();
                       
                    }
                    break;
                case State.Attacking:
                        
                    if (_target != null) 
                    {
                        LookTarget();
                        if (!IsInRange())
                        {
                           // _animationsController.SetMovingState(true);
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
    // private void OnDrawGizmos()
    // {
    //     if (_target != null)
    //     {
    //         Gizmos.color = Color.red;
    //         Gizmos.DrawSphere(_destination, 0.5f); // Dibuja una esfera roja en el destino
    //     }
    //
    //     if (_agent != null && _agent.hasPath)
    //     {
    //         Gizmos.color = Color.red;
    //         Vector3[] path = _agent.path.corners;
    //
    //         for (int i = 0; i < path.Length - 1; i++)
    //         {
    //             Gizmos.DrawLine(path[i], path[i + 1]); // Dibuja líneas verdes para la trayectoria
    //         }
    //     }
    //     
    // }
        
    private void Chase()
    {
        if (_target != null && _state != State.Attacking)
        {
            //_animationsController.SetMovingState(true);
            SetDestination(_target);
        }

        if (_target == null)
        {
            ChangeTarget();
        }
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
           // _animationsController.SetMovingState(false);
            _animationsController.Attack();
            _damageable.TakeDamage(_damage);
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
    private void ChangeTarget()
    {
        _target = null;
        _agent.isStopped = true;
        int doorCount = EnemyController.Instance.Doors.Count;
        GameObject closestGameObject = null;
        float closestDistance = Mathf.Infinity;
        float currentDistance;
        if (doorCount != 0)
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
        //Debug.Log($"Cambiando target a {closestGameObject}");
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
        _damageable = _target?.GetComponent<IDamageable>();
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
        
    private void StartAttacking()
    {
        if (_attackCoroutine == null)
        {
            _agent.isStopped = true;
            _attackCoroutine = StartCoroutine(AttackPerform());
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
        float distanceToDestination = Vector3.Distance(transform.position, _target.transform.position);

        return distanceToDestination <= attackRange;
    }

    private void LookTarget()
    {
        if (_state == State.Attacking  && _target != null) //mira al obeetivo
        {
            _direction = _target.transform.position - transform.position;
            _targetRotation = Quaternion.LookRotation(_direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
        
    private IEnumerator AttackPerform()
    {
        while (_state == State.Attacking)
        {
            Attack(_target);
            yield return new WaitForSeconds(10/attackSpeed);
        }
    }
        
}
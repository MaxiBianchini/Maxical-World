using System.Collections;
using Common;
using Enemys;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class DestroyerMovement : MonoBehaviour, IEnemy, IDamageable
{
    [SerializeField] private float attackRange;
    [SerializeField] private float attackSpeed;

    private HealthBar _healthBar;
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
        _healthBar = GetComponentInChildren<HealthBar>();
    }

    private void Start()
    {
        StopAttacking();

        _maxHealth = _health;
        GetComponentInChildren<HealthBar>().SetMaxHealthValue(_maxHealth);
        _healthBar.UpdateHealthBar(_health);
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
        _healthBar.UpdateHealthBar(_health);
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
        if (_target == null)
        {
            ChangeTarget();
        }
        float distanceToDestination = Vector3.Distance(transform.position, _destination);
            
        if (distanceToDestination <= attackRange && !_isAttacking)
        {
            _isAttacking = true;
            _agent.isStopped = true;
            StartAttacking();
        }
            
        else if (distanceToDestination > attackRange)
        {
            SetDestination(_target);
            _isAttacking = false;
            _animationsController.SetMovingState(true);
            _agent.isStopped = false;
            StopAttacking();
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
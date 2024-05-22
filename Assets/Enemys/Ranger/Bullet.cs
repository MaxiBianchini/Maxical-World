using System;
using System.Collections;
using Common;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemys.Ranger
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private LayerMask damageables;
        private float _damage;
        private float _speed;
        private Vector3 _targetPosition;
        private IDamageable _target;

        private Coroutine _destroyCoroutine;

        public void Initialize(float damage, float speed, IDamageable target, Vector3 targetPosition)
        {
            _damage = damage;
            _speed = speed;
            _target = target;
            _targetPosition = targetPosition;
        }

        private void Update()
        {
            if (_target != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((damageables.value & (1 << other.gameObject.layer)) != 0)
            {
                IDamageable damageable = other.GetComponent<IDamageable>();
                
                if (damageable != null)
                {
                    Impact();
                }
            }
        }

        private void Impact()
        {
            _target.TakeDamage(_damage);
            DestroyBullet();
        }
        private void DestroyBullet()
        {
            Destroy(gameObject);
        }
        
    }
}

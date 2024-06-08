using Common;
using Enemys;
using System;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour, IDamageable
    {
        [Header("Settings")]
        [SerializeField] private float playerSpeed = 5.0f;
        [SerializeField] private float health;

        public static bool Dead;
        private CharacterController _controller;
        private Camera _mainCamera;
        private Animator anim;
        private float actualSpeed = 0;

        private bool isMoving = false;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        void Start()
        {
            _controller = GetComponent<CharacterController>();
            _mainCamera = Camera.main;
            Dead = false;
            
        }


        
        void Update()
        {
            Vector3 move = CalculateMovement();
            PerformMovement(move);
            RotatePlayerTowardsMouse();
            
        }

        private void LateUpdate()
        {
            SetMovingAnimation();
        }

        private void SetMovingAnimation()
        {
            
           anim.SetFloat("xVelocity", actualSpeed);
        }

        private Vector3 CalculateMovement()
        {
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            move = Vector3.ClampMagnitude(move, 1.0f);
            move *= playerSpeed;
            actualSpeed = move.magnitude;
            return move;
        }

        private void PerformMovement(Vector3 move)
        {
            _controller.Move(move * Time.deltaTime);
            if(actualSpeed != 0)
            {
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }
        }

        private void RotatePlayerTowardsMouse()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayDistance;

            if (groundPlane.Raycast(ray, out rayDistance))
            {
                Vector3 point = ray.GetPoint(rayDistance);
                Vector3 heightCorrectedPoint = new Vector3(point.x, transform.position.y, point.z);
                transform.LookAt(heightCorrectedPoint);
            }
        }

        public void TakeDamage(float amount)
        {
           // Debug.Log("PLayter recibico danio " + amount);
           health -= amount;
           if (health <= 0)
           {
               Die();
           }
        }

        private void Die()
        {
            Debug.Log("Murio");
            EnemyController.Instance.SetPlayerDeath();
            Destroy(gameObject);
        }
    }
}
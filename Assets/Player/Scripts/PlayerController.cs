using UnityEngine;

namespace Player.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float playerSpeed = 5.0f;

        private CharacterController _controller;
        private Camera _mainCamera;

        void Start()
        {
            _controller = GetComponent<CharacterController>();
            _mainCamera = Camera.main;
        }

        void Update()
        {
            Vector3 move = CalculateMovement();
            PerformMovement(move);
            RotatePlayerTowardsMouse();
        }

        private Vector3 CalculateMovement()
        {
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            move = Vector3.ClampMagnitude(move, 1.0f);
            move *= playerSpeed;
            return move;
        }

        private void PerformMovement(Vector3 move)
        {
            _controller.Move(move * Time.deltaTime);
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
    }
}
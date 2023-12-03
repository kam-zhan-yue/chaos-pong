using System.Collections;
using System.Collections.Generic;
using ChaosPong.Common;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallDebugger : MonoBehaviour
{
    [SerializeField] private float launchSpeed = 5f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float aimSpeed = 5f;

    private PlayerControls _playerControls;

    private Vector3 _aim = Vector3.forward;
    private Vector3 _movementVector = Vector3.zero;
    private Vector3 _aimVector = Vector3.zero;

    private void Start()
    {
        _playerControls = new PlayerControls();
        _playerControls.Player.Move.performed += Move;
        _playerControls.Player.MoveSpecial.performed += Aim;
        _playerControls.Enable();
    }

    private void Move(InputAction.CallbackContext callbackContext)
    {
        Vector2 movementInput = callbackContext.ReadValue<Vector2>();
        float horizontal = movementInput.x;
        float vertical = movementInput.y;
        _movementVector.x = horizontal;
        _movementVector.z = vertical;
    }

    private void Aim(InputAction.CallbackContext callbackContext)
    {
        Vector2 movementInput = callbackContext.ReadValue<Vector2>();
        float horizontal = movementInput.x;
        float vertical = movementInput.y;
        _aimVector.y = horizontal;
        _aimVector.x = -vertical;
    }

    private Vector3 ProcessMovementVector()
    {
        Transform transform1 = transform;
        Vector3 forward = transform1.forward * _movementVector.z;
        Vector3 right = transform1.right * _movementVector.x;
        return forward + right;
    }

    private void MovePosition()
    {
        Vector3 newPosition = transform.position + ProcessMovementVector() * (moveSpeed * Time.deltaTime);
        transform.position = newPosition;
        
        // Calculate the rotation based on input
        Vector3 rotation = _aimVector * aimSpeed;

        // Apply rotation to the transform
        Transform transform1;
        (transform1 = transform).Rotate(rotation * Time.deltaTime, Space.World);
        
        // Ensure rotation is constrained to the X and Y axes
        Vector3 currentRotation = transform1.eulerAngles;
        currentRotation.z = 0f; // Lock rotation around the Z axis
        transform1.eulerAngles = currentRotation;
    }

    private void Update()
    {
        MovePosition();
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
            IPhysicsService physicsService = ServiceLocator.Instance.Get<IPhysicsService>();
            Transform transform1 = transform;
            physicsService.Projection(transform1.position, transform1.forward * launchSpeed);
        // }
    }
}

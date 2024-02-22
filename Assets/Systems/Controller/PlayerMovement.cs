using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, IMovement
{
    [SerializeField] private float speed;

    private Rigidbody _rigidbody;
    private Vector3 _movementVector = Vector3.zero;
    private float _speedModifier = 1f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Move(InputAction.CallbackContext callbackContext)
    {
        Vector2 movementInput = callbackContext.ReadValue<Vector2>();
        float horizontal = movementInput.x;
        float vertical = movementInput.y;
        _movementVector.x = horizontal;
        _movementVector.z = vertical;
    }

    public void SetActive(bool active)
    {
        enabled = active;
    }

    public void ModifySpeed(float modifer)
    {
        _speedModifier = modifer;
    }

    private Vector3 ProcessMovementVector()
    {
        Transform transform1 = transform;
        Vector3 forward = transform1.forward * _movementVector.z;
        Vector3 right = transform1.right * _movementVector.x;
        return forward + right;
    }

    private void FixedUpdate()
    {
        Vector3 newPosition = transform.position + ProcessMovementVector() * (speed * _speedModifier * Time.deltaTime);
        _rigidbody.MovePosition(newPosition);
    }
}

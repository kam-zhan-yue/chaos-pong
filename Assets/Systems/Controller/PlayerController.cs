using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    
    private PlayerControls _playerControls;
    private Rigidbody _rigidbody;
    private Vector3 _movementVector = Vector3.zero;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _playerControls = new PlayerControls();
        _playerControls.Player.Move.performed += Move;
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

    private void FixedUpdate()
    {
        _rigidbody.velocity = _movementVector * (speed * Time.deltaTime);
        Vector3 newPosition = transform.position + _movementVector * (speed * Time.deltaTime);
        _rigidbody.MovePosition(newPosition);
    }

    private void OnDestroy()
    {
        _playerControls.Player.Move.started -= Move;
        _playerControls.Dispose();
    }
}

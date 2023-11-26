using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IPlayer
{
    [SerializeField] private float speed;

    private ControlScheme _controlScheme;
    private PlayerControls _playerControls;
    private Rigidbody _rigidbody;
    private Vector3 _movementVector = Vector3.zero;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void InitPlayer(PlayerInfo playerInfo)
    {
        _controlScheme = playerInfo.controlScheme;
        Init();
    }

    private void Init()
    {
        _playerControls = new PlayerControls();
        if (_controlScheme == ControlScheme.KeyboardSpecial)
        {
            _playerControls.Player.MoveSpecial.performed += Move;
        }
        else
        {
            _playerControls.Player.Move.performed += Move;
        }
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

    private Vector3 ProcessMovementVector()
    {
        Transform transform1 = transform;
        Vector3 forward = transform1.forward * _movementVector.z;
        Vector3 right = transform1.right * _movementVector.x;
        return forward + right;
    }

    private void FixedUpdate()
    {
        Vector3 newPosition = transform.position + ProcessMovementVector() * (speed * Time.deltaTime);
        _rigidbody.MovePosition(newPosition);
    }

    private void OnDestroy()
    {
        if (_controlScheme == ControlScheme.KeyboardSpecial)
        {
            _playerControls.Player.MoveSpecial.performed -= Move;
        }
        else
        {
            _playerControls.Player.Move.performed -= Move;
        }
        _playerControls.Dispose();
    }
}

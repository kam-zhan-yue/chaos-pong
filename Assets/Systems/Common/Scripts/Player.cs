using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    private IPaddle _paddle;
    private IMovement _movement;

    private PlayerControls _playerControls;
    private PlayerState _state = PlayerState.Idle;

    protected override void Awake()
    {
        base.Awake();
        _movement = GetComponent<IMovement>();
        _paddle = GetComponentInChildren<IPaddle>();
    }
    
    public override void Init(PlayerInfo info)
    {
        base.Init(info);
        InitControls();
        _paddle.Init(playerInfo.teamSide);
    }

    private void InitControls()
    {
        _playerControls = new PlayerControls();
        if (playerInfo.controlScheme == ControlScheme.KeyboardSpecial)
        {
            if(_movement != null)
                _playerControls.Player.MoveSpecial.performed += _movement.Move;
            if(_paddle != null)
                _playerControls.Player.HitSpecial.performed += Hit;
        }
        else
        {
            if(_movement != null)
                _playerControls.Player.Move.performed += _movement.Move;
            if(_paddle != null)
                _playerControls.Player.Hit.performed += Hit;
        }
        _playerControls.Enable();
    }

    private void Hit(InputAction.CallbackContext callbackContext)
    {
        switch (_state)
        {
            //Do nothing
            case PlayerState.Idle:
                break;
            //Throw the pong into the air and wait for next input
            case PlayerState.Starting:
                _movement.SetActive(false);
                _paddle.Toss();
                _state = PlayerState.Serving;
                break;
            //Serve the pong
            case PlayerState.Serving:
                _movement.SetActive(true);
                _paddle.Serve();
                _state = PlayerState.Returning;
                break;
            //Return the pong
            case PlayerState.Returning:
                _paddle.Return();
                break;
        }
    }

    public override void SetStart()
    {
        _paddle.SetStart();
        _state = PlayerState.Starting;
    }
    
    private void OnDestroy()
    {
        _playerControls.Dispose();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private IPaddle _paddle;
    private IMovement _movement;

    private PlayerControls _playerControls;

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
    }

    private void InitControls()
    {
        _playerControls = new PlayerControls();
        if (playerInfo.controlScheme == ControlScheme.KeyboardSpecial)
        {
            if(_movement != null)
                _playerControls.Player.MoveSpecial.performed += _movement.Move;
            if(_paddle != null)
                _playerControls.Player.HitSpecial.performed += _paddle.Return;
        }
        else
        {
            if(_movement != null)
                _playerControls.Player.Move.performed += _movement.Move;
            if(_paddle != null)
                _playerControls.Player.Hit.performed += _paddle.Return;
        }
        _playerControls.Enable();
    }

    public override void SetServe()
    {
        if (_paddle != null)
        {
            _paddle.SetServe();
        }
    }
    
    private void OnDestroy()
    {
        _playerControls.Dispose();
    }
}

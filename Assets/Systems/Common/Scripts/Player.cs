using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private IPlayer[] _playerComponents = Array.Empty<IPlayer>();
    private IPaddle _paddle;
    private IMovement _movement;

    private PlayerControls _playerControls;
    private PlayerInfo _playerInfo = new();

    private void Awake()
    {
        _playerComponents = GetComponents<IPlayer>();
        _movement = GetComponent<IMovement>();
        _paddle = GetComponentInChildren<IPaddle>();
    }
    
    public void Init(PlayerInfo playerInfo)
    {
        _playerInfo = playerInfo;
        if(!string.IsNullOrEmpty(playerInfo.id))
            gameObject.name = playerInfo.id;
        if (_playerComponents.Length > 0)
        {
            for (int i = 0; i < _playerComponents.Length; ++i)
            {
                _playerComponents[i].InitPlayer(playerInfo);
            }
        }
        InitControls();
    }

    private void InitControls()
    {
        _playerControls = new PlayerControls();
        if (_playerInfo.controlScheme == ControlScheme.KeyboardSpecial)
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

    public void SetServe()
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

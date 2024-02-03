using System;
using System.Collections;
using System.Collections.Generic;
using Codice.CM.Client.Differences;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class Trainer : Character
{
    private PlayerControls _playerControls;
    
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
            _playerControls.Player.MoveSpecial.performed += Aim;
            _playerControls.Player.HitSpecial.performed += Hit;
        }
        else
        {
            _playerControls.Player.Move.performed += Aim;
            _playerControls.Player.Hit.performed += Hit;
        }
        _playerControls.Enable();
    }

    private void Aim(InputAction.CallbackContext callbackContext)
    {
        
    }

    private void Hit(InputAction.CallbackContext callbackContext)
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided!");
    }

    public override void SetServe()
    {
        
    }
}

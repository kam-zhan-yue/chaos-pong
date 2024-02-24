using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inferno : Ability, IAbilitySpecial
{
    private Animator _animator;
    private CinemachineStateDrivenCamera _stateCamera;
    private static readonly int ActivateTrigger = Animator.StringToHash("Activate");

    
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _stateCamera = GetComponentInChildren<CinemachineStateDrivenCamera>();
        _stateCamera.enabled = false;
    }

    protected override void Activate()
    {
        Time.timeScale = 0f;
        _stateCamera.enabled = true;
        _animator.SetTrigger(ActivateTrigger);
    }

    protected override void Deactivate()
    {
        Time.timeScale = 1f;
        _stateCamera.enabled = false;
    }

    public void Activate(InputAction.CallbackContext callbackContext)
    {
        ProcessInput();
    }
}
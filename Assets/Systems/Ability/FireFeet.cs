using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireFeet : Ability, IAbilityPrimary
{
    [SerializeField] private float speedModifier = 2f;
    private IMovement _movement;
    
    private void Awake()
    {
        _movement = GetComponentInParent<IMovement>();
    }
    public void Activate(InputAction.CallbackContext callbackContext)
    {
        ProcessInput();
    }

    protected override void Activate()
    {
        _movement.ModifySpeed(speedModifier);
    }

    protected override void Deactivate()
    {
        _movement.ModifySpeed(1f);
    }
}

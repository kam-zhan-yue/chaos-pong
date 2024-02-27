using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireFeet : Ability, IAbilityPrimary
{
    [SerializeField] private float speedModifier = 2f;
    private IMovement _movement;
    private Player _player;
    
    private void Awake()
    {
        _player = GetComponentInParent<Player>();
        _movement = GetComponentInParent<IMovement>();
    }
    
    public void Activate(InputAction.CallbackContext callbackContext)
    {
        ProcessInput();
    }

    protected override bool Interactive()
    {
        return base.CanActivate() && _player.State == PlayerState.Returning;
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

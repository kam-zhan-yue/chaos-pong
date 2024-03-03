using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireHand : Ability, IAbilitySecondary
{
    [SerializeField] private HitModifier hitModifier;
    private IPaddle _paddle;
    private Player _player;

    private void Awake()
    {
        _paddle = transform.parent.GetComponentInChildren<IPaddle>();
        _player = GetComponentInParent<Player>();
    }

    public void Activate(InputAction.CallbackContext callbackContext)
    {
        ProcessInput();
    }

    protected override bool Interactive()
    {
        return base.CanActivate() && _player.State == CharacterState.Returning;
    }

    protected override void Activate()
    {
        _paddle.SetHitModifier(hitModifier);
    }

    protected override void Deactivate()
    {
    }

}
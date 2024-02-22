using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireHand : Ability, IAbilitySecondary
{
    [SerializeField] private HitModifier hitModifier;
    private IPaddle _paddle;

    private void Start()
    {
        Player player = GetComponentInParent<Player>();
        _paddle = player.Paddle;
    }

    public void Activate(InputAction.CallbackContext callbackContext)
    {
        ProcessInput();
    }
    
    protected override void Activate()
    {
        _paddle.SetHitModifier(hitModifier);
    }

    protected override void Deactivate()
    {
    }

}
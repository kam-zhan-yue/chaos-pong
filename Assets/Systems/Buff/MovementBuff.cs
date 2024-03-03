using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBuff : Buff
{
    private readonly IMovement _movement;
    private readonly float _speedModifier;
    
    public MovementBuff(IMovement movement, float speedModifier, float duration) : base(duration)
    {
        _movement = movement;
        _speedModifier = speedModifier;
    }
    
    protected override void ApplyEffect()
    {
        _movement.ModifySpeed(_speedModifier);
    }

    protected override void RemoveEffect()
    {
        _movement.ModifySpeed(1f);
    }
}

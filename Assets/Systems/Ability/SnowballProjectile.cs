using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballProjectile : Projectile
{
    [SerializeField] private float speedModifier;
    [SerializeField] private float duration;
    
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Character character))
        {
            Debug.Log("Hit");
            ApplyBuff(character);
        }

        //Destroy if not pong
        if (!other.gameObject.TryGetComponent(out Pong pong))
        {
            Destroy(gameObject);
        }
    }

    private void ApplyBuff(Character character)
    {
        if (character.TryGetComponent(out BuffController buffController))
        {
            if (character.TryGetComponent(out IMovement movement))
            {
                MovementBuff movementBuff = new MovementBuff(movement, speedModifier, duration);
                buffController.ApplyBuff(movementBuff);
            }
        }
    }
}

using System;
using UnityEngine;

[Serializable]
public class HitModifier
{
    public float strength = 0f;

    public Vector3 ModifyAcceleration(Vector3 acceleration)
    {
        float gravity = Physics.gravity.y * strength;
        return new Vector3(acceleration.x, gravity, acceleration.z);
    }
}
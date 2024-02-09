using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public struct HitInfo
{
    public Vector3 position;
    public bool serve;
    public Vector3 velocity;
    public TeamSide teamSide;

    public HitInfo(bool serve, Vector3 position, Vector3 velocity, TeamSide teamSide)
    {
        this.serve = serve;
        this.position = position;
        this.velocity = velocity;
        this.teamSide = teamSide;
    }
}
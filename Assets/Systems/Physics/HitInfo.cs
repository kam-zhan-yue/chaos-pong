using UnityEngine;

public struct HitInfo
{
    public bool serve;
    public Vector3 velocity;
    public TeamSide teamSide;

    public HitInfo(bool serve, Vector3 velocity, TeamSide teamSide)
    {
        this.serve = serve;
        this.velocity = velocity;
        this.teamSide = teamSide;
    }
}
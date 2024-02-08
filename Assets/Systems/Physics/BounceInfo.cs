using UnityEngine;

public struct BounceInfo
{
    public TeamSide teamSide;
    public Vector3 velocity;

    public BounceInfo(TeamSide teamSide, Vector3 velocity)
    {
        this.teamSide = teamSide;
        this.velocity = velocity;
    }
}
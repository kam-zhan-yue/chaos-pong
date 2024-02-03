using UnityEngine;

public interface IPhysicsService : IGameService
{
    public void Projection(Vector3 position, Vector3 velocity, TeamSide teamSide = TeamSide.None);
    public void ServeBall(Vector3 position, Vector3 velocity, TeamSide teamSide = TeamSide.None);
    public void HideProjection();
}

using UnityEngine;

public interface IPhysicsService : IGameService
{
    public void Projection(Vector3 position, Vector3 velocity);
    public void ServeBall(Vector3 position, Vector3 velocity);
    public void HideProjection();
}

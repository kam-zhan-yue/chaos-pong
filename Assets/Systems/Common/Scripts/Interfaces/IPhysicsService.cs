using UnityEngine;

public interface IPhysicsService : IGameService
{
    public void Projection(Vector3 position, Vector3 velocity);
}

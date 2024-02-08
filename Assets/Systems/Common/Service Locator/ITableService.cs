using UnityEngine;

public interface ITableService : IGameService
{
    public float Height();
    public bool InBounds(Vector3 position);
    public Vector3 GetRandomPoint(TeamSide teamSide);
    public TeamSide GetTeamSide(Vector3 position);
}
using Kuroneko.UtilityDelivery;
using UnityEngine;

public interface ITableService : IGameService
{
    public float Height();
    public bool InBounds(Vector3 position);
    public Vector3 GetRandomPoint(TeamSide teamSide);
    public Vector3 GetServePoint(TeamSide servingSide, Vector3 ballPosition);
    public TeamSide GetTeamSide(Vector3 position);
}
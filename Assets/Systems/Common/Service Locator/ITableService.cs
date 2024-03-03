using Kuroneko.UtilityDelivery;
using UnityEngine;

public interface ITableService : IGameService
{
    public float Height();
    public bool InBounds(Vector3 position);
    public Vector3 GetRandomPoint(TeamSide teamSide);
    public Vector3 GetServePoint(TeamSide servingSide, Vector3 ballPosition);
    public TeamSide GetTeamSide(Vector3 position);
    public Vector3 Center();
    
    /// <summary>
    /// Returns the direction towards the team from the center
    /// </summary>
    /// <param name="teamSide"></param>
    /// <returns></returns>
    public Vector3 TeamDirection(TeamSide teamSide);
}
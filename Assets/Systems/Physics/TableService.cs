using System.Collections;
using System.Collections.Generic;
using ChaosPong.Common;
using UnityEngine;
using UnityEngine.Animations;

public class TableService : MonoBehaviour, ITableService
{
    [SerializeField] private float width = 2f;
    [SerializeField] private float length = 2f;
    
    public float Height() => transform.position.y * transform.localScale.y;

    private void Awake()
    {
        ServiceLocator.Instance.Register<ITableService>(this);
    }

    private Bounds GetBounds()
    {
        Vector3 size = new(length, 0f, width);
        return new Bounds(transform.position, size);
    }

    public bool InBounds(Vector3 point)
    {
        Bounds bounds = GetBounds();
        return bounds.Contains(new Vector3(point.x, bounds.center.y, point.z));
    }

    public TeamSide GetTeamSide(Vector3 point)
    {
        if (!InBounds(point))
            return TeamSide.None;

        Bounds localBounds = GetBounds();
        float tableLength = GetLength(localBounds);
        Vector3 redCenter = GetRedCenter(localBounds);
        Vector3 blueCenter = GetBlueCenter(localBounds);
        Vector3 size = localBounds.size;
        size.x = tableLength;
        Bounds redBounds = new(redCenter, size);
        Bounds blueBounds = new(blueCenter, size);
        if (redBounds.Contains(new Vector3(point.x, redBounds.center.y, point.z)))
            return TeamSide.Red;
        if (blueBounds.Contains(new Vector3(point.x, blueBounds.center.y, point.z)))
            return TeamSide.Blue;
        return TeamSide.None;
    }

    public Vector3 GetRandomPoint(TeamSide teamSide)
    {
        Vector3 center = Vector3.zero;
        Bounds localBounds = GetBounds();
        switch (teamSide)
        {
            case TeamSide.Blue:
                center = GetBlueHitCenter(localBounds);
                break;
            case TeamSide.Red:
                center = GetRedHitCenter(localBounds);
                break;
            default:
                return center;
        }

        Vector3 size = localBounds.size;
        size.x = GetLength(localBounds) * 0.25f;
        Bounds bounds = new(center, size);
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);
        return new Vector3(randomX, bounds.center.y, randomZ);
    }

    private Vector3 GetBlueCenter(Bounds bounds)
    {
        float tableLength = GetLength(bounds);
        Vector3 position = transform.position;
        float x = position.x;
        position.x = x + tableLength * 0.5f;
        return position;
    }

    private Vector3 GetRedCenter(Bounds bounds)
    {
        float tableLength = GetLength(bounds);
        Vector3 position = transform.position;
        float x = position.x;
        position.x = x - tableLength * 0.5f;
        return position;
    }

    private Vector3 GetBlueHitCenter(Bounds bounds)
    {
        Vector3 center = GetBlueCenter(bounds);
        center.x += GetLength(bounds) * 0.25f;
        return center;
    }

    private Vector3 GetRedHitCenter(Bounds bounds)
    {
        Vector3 center = GetRedCenter(bounds);
        center.x -= GetLength(bounds) * 0.25f;
        return center;
    }

    private float GetLength(Bounds bounds)
    {
        return bounds.size.x * 0.5f;
    }

    private void OnDrawGizmos()
    {
        Bounds bounds = GetBounds();
        float tableLength = GetLength(bounds);
        Vector3 bluePos = GetBlueCenter(bounds);
        Vector3 redPos = GetRedCenter(bounds);
        bluePos.y += 0.001f;
        redPos.y += 0.001f;
        
        Vector3 blueHitPos = GetBlueHitCenter(bounds);
        Vector3 redHitPos = GetRedHitCenter(bounds);
        blueHitPos.y += 0.002f;
        redHitPos.y += 0.002f;
        
        Gizmos.color = ChaosPongHelper.Blue;
        Gizmos.DrawCube(bluePos, new Vector3(tableLength, bounds.size.y, bounds.size.z));
        Gizmos.color = ChaosPongHelper.Red;
        Gizmos.DrawCube(redPos, new Vector3(tableLength, bounds.size.y, bounds.size.z));

        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(blueHitPos, new Vector3(tableLength * 0.5f, bounds.size.y, bounds.size.z));
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(redHitPos, new Vector3(tableLength * 0.5f, bounds.size.y, bounds.size.z));
    }
}

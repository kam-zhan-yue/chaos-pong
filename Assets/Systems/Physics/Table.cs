using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Table : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    public float Height => transform.position.y * transform.localScale.y;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public bool InBounds(Vector3 point)
    {
        Bounds localBounds = _meshRenderer.bounds;
        Bounds worldBounds = new(transform.TransformPoint(localBounds.center), localBounds.size);
        return worldBounds.Contains(new Vector3(point.x, worldBounds.center.y, point.z));
    }

    public TeamSide GetTeamSide(Vector3 point)
    {
        if (!InBounds(point))
            return TeamSide.None;

        Bounds localBounds = _meshRenderer.bounds;
        float length = GetLength(localBounds);
        Vector3 redCenter = GetRedCenter(localBounds);
        Vector3 blueCenter = GetBlueCenter(localBounds);
        Vector3 size = localBounds.size;
        size.x = length;
        Bounds redBounds = new(redCenter, size);
        Bounds blueBounds = new(blueCenter, size);
        if (redBounds.Contains(new Vector3(point.x, redBounds.center.y, point.z)))
            return TeamSide.Red;
        if (blueBounds.Contains(new Vector3(point.x, blueBounds.center.y, point.z)))
            return TeamSide.Blue;
        return TeamSide.None;
    }

    private Vector3 GetBlueCenter(Bounds bounds)
    {
        float length = GetLength(bounds);
        Vector3 position = transform.position;
        float x = position.x;
        position.x = x + length * 0.5f;
        return position;
    }

    private Vector3 GetRedCenter(Bounds bounds)
    {
        float length = GetLength(bounds);
        Vector3 position = transform.position;
        float x = position.x;
        position.x = x - length * 0.5f;
        return position;
    }

    private float GetLength(Bounds bounds)
    {
        return bounds.size.x * 0.5f;
    }

    private void OnDrawGizmos()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        Bounds bounds = meshRenderer.bounds;
        float length = GetLength(bounds);
        Vector3 bluePos = GetBlueCenter(bounds);
        Vector3 redPos = GetRedCenter(bounds);
        bluePos.y += 0.001f;
        redPos.y += 0.001f;
        Gizmos.color = ChaosPongHelper.Blue;
        Gizmos.DrawCube(bluePos, new Vector3(length, bounds.size.y, bounds.size.z));
        Gizmos.color = ChaosPongHelper.Red;
        Gizmos.DrawCube(redPos, new Vector3(length, bounds.size.y, bounds.size.z));
    }
}

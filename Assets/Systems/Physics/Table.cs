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
        Bounds worldBounds = new Bounds(transform.TransformPoint(localBounds.center), localBounds.size);
        Debug.Log($"Point: {point}");
        return worldBounds.Contains(new Vector3(point.x, worldBounds.center.y, point.z));
    }
}

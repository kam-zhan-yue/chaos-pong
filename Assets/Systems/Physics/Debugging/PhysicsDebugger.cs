using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Physics Debugger")]
public class PhysicsDebugger : ScriptableObject
{
    public Pong pongPrefab;
    public List<HitInfo> hits;

    public void SaveHits(List<HitInfo> hitList)
    {
        hits = hitList;
    }

    [Button]
    public void DebugHit(int index)
    {
        if (index > hits.Count)
        {
            return;
        }

        HitInfo hitInfo = hits[index];
        Pong pong = Instantiate(pongPrefab, hits[index].position, Quaternion.identity);
        pong.DebugHitInfo(hitInfo);
    }

}
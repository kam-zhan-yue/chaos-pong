using System.Collections;
using System.Collections.Generic;
using Kuroneko.UtilityDelivery;
using Sirenix.OdinInspector;
using UnityEngine;

public class IcyFloor : MonoBehaviour
{
    public float width = 5f;
    public float height = 5f;
    public float returnHeight = 3f;
    private TeamSide _teamSide;
    
    [Button]
    public void Init(TeamSide teamSide, Vector3 position)
    {
        _teamSide = teamSide;
        transform.position = position;

    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log($"Triggered with {other.gameObject.name}");
        if (other.gameObject.TryGetComponent(out Pong pong))
        {
            pong.Return(_teamSide, returnHeight);
        }
    }
}

using System;
using ChaosPong.Common;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class Paddle : MonoBehaviour, IPaddle
{
    [SerializeField] private Pong pongPrefab;
    [SerializeField] private Transform serveTransform;
    private SphereCollider[] _colliders = Array.Empty<SphereCollider>();
    // private PaddleState _paddleState = PaddleState.Idle;
    private Collider[] _pongHits = new Collider[10];
    private Pong _pong;
    private TeamSide _teamSide;

    private void Awake()
    {
        _colliders = GetComponentsInChildren<SphereCollider>();
    }

    public void Init(TeamSide teamSide)
    {
        _teamSide = teamSide;
    }

    public void SetStart()
    {
        _pong = Instantiate(pongPrefab, serveTransform);
    }

    public void Toss()
    {
        _pong.Toss();
    }

    public void Serve()
    {
        _pong.Serve(_teamSide, ChaosPongHelper.SERVE_HEIGHT);
    }

    public void Return()
    {
        if (TryGetPong(out Pong pong))
        {
            pong.Return(_teamSide, ChaosPongHelper.RETURN_HEIGHT);
        }
    }

    private bool TryGetPong(out Pong pong)
    {
        for (int i = 0; i < _colliders.Length; ++i)
        {
            int count = Physics.OverlapSphereNonAlloc(_colliders[i].transform.position, _colliders[i].radius, _pongHits);
            for (int j = 0; j < count; ++j)
            {
                if (_pongHits[j].gameObject.TryGetComponent(out Pong pongComponent))
                {
                    pong = pongComponent;
                    return true;
                }
            }
        }

        pong = null;
        return false;
    }
}

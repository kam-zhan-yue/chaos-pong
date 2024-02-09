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
    private PaddleState _paddleState = PaddleState.Idle;
    private Collider[] _pongHits = new Collider[10];
    private Pong _pong;

    private void Awake()
    {
        _colliders = GetComponentsInChildren<SphereCollider>();
    }
    
    public void SetServe()
    {
        _paddleState = PaddleState.Serve;
        _pong = Instantiate(pongPrefab, serveTransform);
    }

    private void Serve(TeamSide teamSide)
    {
        _pong.Serve(teamSide, ChaosPongHelper.SERVE_HEIGHT);
        _paddleState = PaddleState.Idle;
    }

    public void Return(TeamSide teamSide)
    {
        if (_paddleState == PaddleState.Serve)
        {
            Serve(teamSide);
        }
        else
        {
            Hit(teamSide);
        }
    }

    [Button]
    private void Hit(TeamSide teamSide)
    {
        if (TryGetPong(out Pong pong))
        {
            pong.Return(teamSide, 2f);
        }
    }

    private int GetBalls(Vector3 position, float radius)
    {
        int ballCount = Physics.OverlapSphereNonAlloc(position, radius, _pongHits);
        Debug.Log($"Ball Count: {ballCount}");
        for (int i = 0; i < ballCount; ++i)
        {
            Debug.Log($"Hit: {_pongHits[i].name}");
        }

        return ballCount;
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

    private async UniTaskVoid ActivateAsync()
    {
        ActivateColliders(true);
        
        await UniTask.WaitForEndOfFrame(this);
        
        ActivateColliders(false);
    }

    private void ActivateColliders(bool active)
    {
        for (int i = 0; i < _colliders.Length; ++i)
        {
            _colliders[i].enabled = active;
        }
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < _colliders.Length; i++)
        {
            Gizmos.DrawSphere(_colliders[i].transform.position, _colliders[i].radius);
        }
    }
}

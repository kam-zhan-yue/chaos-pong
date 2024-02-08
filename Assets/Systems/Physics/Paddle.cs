using System;
using ChaosPong.Common;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Paddle : MonoBehaviour, IPaddle
{
    [SerializeField] private Pong pongPrefab;
    [SerializeField] private Transform serveTransform;
    private SphereCollider[] _colliders = Array.Empty<SphereCollider>();
    private PaddleState _paddleState = PaddleState.Idle;
    private RaycastHit[] _ballHits = new RaycastHit[10];
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

    private void Hit(TeamSide teamSide)
    {
        Debug.Log("Hit!");
        for (int i = 0; i < _colliders.Length; ++i)
        {
            int ballCount = GetBalls(_colliders[i].transform.position, _colliders[i].radius);
        }
        
    }

    private int GetBalls(Vector3 position, float radius)
    {
        int ballCount = Physics.SphereCastNonAlloc(position, radius, Vector3.zero, _ballHits);
        Debug.Log($"Ball Count: {ballCount}");
        for (int i = 0; i < ballCount; ++i)
        {
            Debug.Log($"Hit: {_ballHits[i].collider.name}");
        }

        return ballCount;
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

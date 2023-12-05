using System;
using ChaosPong.Common;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Paddle : MonoBehaviour, IPaddle
{
    [SerializeField] private Transform serveTransform;
    [SerializeField] private float serveForce = 5f;
    private SphereCollider[] _colliders = Array.Empty<SphereCollider>();
    private PaddleState _paddleState = PaddleState.Idle;

    private void Awake()
    {
        _colliders = GetComponentsInChildren<SphereCollider>();
    }

    private void Start()
    {
        ActivateColliders(false);
    }

    private void Update()
    {
        switch (_paddleState)
        {
            case PaddleState.Idle:
                break;
            case PaddleState.Serve:
                ServiceLocator.Instance.Get<IPhysicsService>().Projection(serveTransform.position, serveTransform.forward * serveForce);
                break;
        }
    }

    public void SetServe()
    {
        _paddleState = PaddleState.Serve;
    }

    public void Serve()
    {
        IPhysicsService physicsService = ServiceLocator.Instance.Get<IPhysicsService>();
        physicsService.HideProjection();
        physicsService.ServeBall(serveTransform.position, serveTransform.forward * serveForce);
        _paddleState = PaddleState.Idle;
    }

    public void Return(InputAction.CallbackContext callbackContext)
    {
        if (_paddleState == PaddleState.Serve)
        {
            Serve();
        }
        else
        {
            Hit();
        }
    }

    private void Hit()
    {
        // for (int i = 0; i < _colliders.Length; ++i)
        // {
        //     Physics.SphereCastNonAlloc()
        // }
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

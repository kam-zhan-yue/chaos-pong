using System;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class Paddle : MonoBehaviour, IPaddle, IPongFinder
{
    [SerializeField] private Pong pongPrefab;
    [SerializeField] private Transform serveTransform;
    private SphereCollider[] _colliders = Array.Empty<SphereCollider>();
    private readonly Collider[] _pongHits = new Collider[10];
    private Pong _pong;
    private TeamSide _teamSide;
    private HitModifier _hitModifier;

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
        IGameManager gameManager = ServiceLocator.Instance.Get<IGameManager>();
        gameManager?.Serve(_teamSide);
        _pong.Serve(_teamSide, ChaosPongHelper.SERVE_HEIGHT);
    }

    public void Return()
    {
        if (TryGetPong(out Pong pong))
        {
            pong.ResetModifier();
            float height = pong.transform.position.y;
            HitType hitType = ChaosPongHelper.GetHitType(height);
            switch (hitType)
            {
                case HitType.Return:
                    Debug.Log($"Return {height}");
                    pong.Return(_teamSide, ChaosPongHelper.RETURN_HEIGHT, HitType.Return, _hitModifier);
                    break;
                case HitType.Smash:
                    Debug.Log($"Smash {height}");
                    pong.Return(_teamSide, ChaosPongHelper.SMASH_HEIGHT, HitType.Smash, _hitModifier);
                    break;
                case HitType.Snake:
                    Debug.Log($"Snake {height}");
                    pong.Return(_teamSide, ChaosPongHelper.SNAKE_HEIGHT, HitType.Snake, _hitModifier);
                    break;
            }
            _hitModifier = new HitModifier();
        }
    }

    public void SetHitModifier(HitModifier modifier)
    {
        _hitModifier = modifier;
    }

    public bool CanHit()
    {
        return TryGetPong(out Pong pong) && pong.CanReturn(_teamSide);
    }

    public bool TryGetPong(out Pong pong)
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

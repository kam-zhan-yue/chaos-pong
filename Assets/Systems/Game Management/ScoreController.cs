using System;
using System.Collections;
using System.Collections.Generic;
using ChaosPong.Common;
using UnityEngine;

public class ScoreController : MonoBehaviour, IScoreService
{
    private void Awake()
    {
        ServiceLocator.Instance.Register<IScoreService>(this);
    }

    private void Start()
    {
        Pong.onHit += OnHit;
        Pong.onBounce += OnBounce;
    }

    private void OnHit(HitInfo hitInfo)
    {
        
    }

    private void OnBounce(BounceInfo bounceInfo)
    {
        
    }

    private void OnDestroy()
    {
        Pong.onHit -= OnHit;
        Pong.onBounce -= OnBounce;
    }
}

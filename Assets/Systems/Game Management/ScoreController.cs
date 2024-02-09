using System;
using System.Collections;
using System.Collections.Generic;
using ChaosPong.Common;
using UnityEngine;

public class ScoreController : MonoBehaviour, IScoreService
{
    private float _blueScore;
    private GameState _gameState;
    private bool _started = false;
    
    private void Awake()
    {
        ServiceLocator.Instance.Register<IScoreService>(this);
    }

    private void Start()
    {
        Pong.onHit += OnHit;
        Pong.onBounce += OnBounce;
    }

    public void StartGame(GameState gameState)
    {
        _started = true;
        _gameState = gameState;
    }

    private void OnHit(HitInfo hitInfo)
    {
        if (!_started)
            return;
        _gameState.Hit(hitInfo.teamSide);
    }

    private void OnBounce(BounceInfo bounceInfo)
    {
        if (!_started)
            return;
        if (bounceInfo.teamSide == TeamSide.None)
        {
            if (_gameState.Possession == TeamSide.Blue)
            {
                _gameState.RedPoint();
            }
            else if (_gameState.Possession == TeamSide.Red)
            {
                _gameState.BluePoint();
            }
        }
    }

    private void OnDestroy()
    {
        Pong.onHit -= OnHit;
        Pong.onBounce -= OnBounce;
    }
}

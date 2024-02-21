using System;
using System.Collections;
using System.Collections.Generic;
using ChaosPong.Common;
using Signals;
using Sirenix.OdinInspector;
using SuperMaxim.Messaging;
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
        Debug.Log("ScoreController Subscribe");
        Messenger.Default.Subscribe<ScorePayload>(OnScore);
        SignalSubscribtionManager manager = new();
        manager.Subscribe(SignalManager.BluePoints, OnBluePointsChanged);
        manager.Subscribe(SignalManager.RedPoints, OnRedPointsChanged);
        manager.Initialize();
    }

    private void OnBluePointsChanged(int current)
    {
        Debug.Log($"Blue Points: {current}");
    }

    private void OnRedPointsChanged(int current)
    {
        Debug.Log($"Red Points: {current}");
    }

    private void OnScore(ScorePayload payload)
    {
        // Debug.Log($"Point for: {payload.TeamSide}");
        if (payload.TeamSide == TeamSide.Blue)
            _gameState.BluePoint();
        else if (payload.TeamSide == TeamSide.Red)
            _gameState.RedPoint();
    }

    private void OnPoint(TeamSide teamSide)
    {
        if (teamSide == TeamSide.Blue)
            _gameState.BluePoint();
        else if (teamSide == TeamSide.Red)
            _gameState.RedPoint();
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
    }
}

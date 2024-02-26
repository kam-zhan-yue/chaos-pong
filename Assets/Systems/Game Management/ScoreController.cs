using System;
using System.Collections;
using System.Collections.Generic;
using Kuroneko.UtilityDelivery;
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
        Messenger.Default.Subscribe<ScorePayload>(OnScore);
    }

    public void StartGame(GameState gameState)
    {
        _started = true;
        _gameState = gameState;
        _gameState.BluePoints.Subscribe(OnBluePointsChanged);
        _gameState.RedPoints.Subscribe(OnRedPointsChanged);
    }
    
    private void OnBluePointsChanged(int prev, int curr)
    {
        // Debug.Log($"Blue Points: {current}");
    }

    private void OnRedPointsChanged(int prev, int curr)
    {
        // Debug.Log($"Red Points: {current}");
    }

    private void OnScore(ScorePayload payload)
    {
        // Debug.Log($"Point for: {payload.TeamSide}");
        if (payload.TeamSide == TeamSide.Blue)
            _gameState.BluePoint();
        else if (payload.TeamSide == TeamSide.Red)
            _gameState.RedPoint();
        Messenger.Default.Publish(new EventPayload(GameEvent.StartRound));
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

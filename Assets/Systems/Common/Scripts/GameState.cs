using System;
using Signals;
using UnityEngine;

[Serializable]
public class GameState
{
    public float pongVelocity;
    public float pongPosition;
    public int rally;
    
    private int _maxRally;
    public TeamSide Possession { get; private set; }
    public TeamSide StartingSide { get; private set; }
    public int Round { get; private set; }
    private Signal<int> _blue = new Signal<int>(0);
    private Signal<int> _red = new Signal<int>(0);

    public GameState(TeamSide startingSide)
    {
        StartingSide = startingSide;
        Round = 1;

        Debug.Log("Set Game State Signals");
        _blue.Value = 0;
        _red.Value = 0;
        SignalManager.BluePoints.UpdateDeps(_blue);
        SignalManager.RedPoints.UpdateDeps(_red);
    }

    public void Hit(TeamSide teamSide)
    {
        Possession = teamSide;
    }

    public void RedPoint()
    {
        _red.Value++;
    }

    public void BluePoint()
    {
        _blue.Value++;
    }
}
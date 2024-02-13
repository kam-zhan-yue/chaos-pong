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
    public Signal<int> blue = new Signal<int>(0);
    public Signal<int> red = new Signal<int>(0);

    public GameState(TeamSide startingSide)
    {
        StartingSide = startingSide;
        Round = 1;

        Debug.Log("Set Game State Signals");
        blue.Value = 0;
        red.Value = 0;
        SignalManager.BluePoints.UpdateDeps(blue);
        SignalManager.RedPoints.UpdateDeps(red);
    }

    public void Hit(TeamSide teamSide)
    {
        Possession = teamSide;
    }

    public void RedPoint()
    {
        red.Value++;
    }

    public void BluePoint()
    {
        blue.Value++;
    }
}
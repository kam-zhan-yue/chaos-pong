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
    public Team RedTeam { get; private set; }
    public Team BlueTeam { get; private set; }
    public int Round { get; private set; }
    public Signal<int> BluePoints { get; set; } = new Signal<int>();
    public Signal<int> RedPoints { get; set; } = new Signal<int>();

    public GameState(TeamSide startingSide, Team redTeam,Team blueTeam)
    {
        //Initialise Team Values
        StartingSide = startingSide;
        Round = 1;
        RedTeam = redTeam;
        BlueTeam = blueTeam;

        //Initialise Signal Values
        BluePoints.Value = 0;
        RedPoints.Value = 0;
    }

    public void Hit(TeamSide teamSide)
    {
        Possession = teamSide;
    }

    public void RedPoint()
    {
        RedPoints.Value++;
    }

    public void BluePoint()
    {
        BluePoints.Value++;
    }

    public Team GetTeam(TeamSide teamSide)
    {
        return teamSide switch
        {
            TeamSide.Red => RedTeam,
            TeamSide.Blue => BlueTeam,
            _ => new(TeamSide.None)
        };
    }
}
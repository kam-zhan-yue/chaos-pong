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

    public bool GamePoint()
    {
        int gamePoint = ChaosPongHelper.GAME_POINT;
        // Check if either team has 10 or more points
        if (BluePoints.Value >= gamePoint-1 || RedPoints.Value >= gamePoint-1)
        {
            // Check if the difference between the points is 1
            if (Mathf.Abs(BluePoints.Value - RedPoints.Value) == 1)
            {
                return true; // It's a game point or advantage
            }
        }
        return false; // Not a game point or advantage
    }

    public TeamSide GetWinner()
    {
        int gamePoint = ChaosPongHelper.GAME_POINT;
        // Check if either team has reached 11 points and has a lead of 2 or more
        if ((BluePoints.Value >= gamePoint || RedPoints.Value >= gamePoint) && Mathf.Abs(BluePoints.Value - RedPoints.Value) >= 2)
        {
            return BluePoints.Value > RedPoints.Value ? TeamSide.Blue : TeamSide.Red;
        }
        // Check if both teams have reached 10 points and one team has a lead of 2 or more
        if ((BluePoints.Value >= gamePoint-1 && RedPoints.Value >= gamePoint-1) && Mathf.Abs(BluePoints.Value - RedPoints.Value) >= 2)
        {
            return BluePoints.Value > RedPoints.Value ? TeamSide.Blue : TeamSide.Red;
        }

        return TeamSide.None;
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
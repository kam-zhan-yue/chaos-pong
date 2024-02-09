using UnityEngine;

public class GameState
{
    public float pongVelocity;
    public float pongPosition;
    public int rally;
    
    private int _maxRally;
    public int BlueScore { get; private set; }
    public int RedScore { get; private set; }
    public TeamSide Possession { get; private set; }
    public TeamSide StartingSide { get; private set; }
    public int Round { get; private set; }

    public GameState(TeamSide startingSide)
    {
        StartingSide = startingSide;
        Round = 1;
    }

    public void GetServer()
    {
    }

    public void Hit(TeamSide teamSide)
    {
        Possession = teamSide;
        // Debug.Log($"Set Possession: {teamSide}");
    }

    public void RedPoint()
    {
        RedScore++;
    }

    public void BluePoint()
    {
        BlueScore++;
    }
}
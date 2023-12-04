using System;
using System.Collections.Generic;

[Serializable]
public class Team
{
    public TeamSide side;
    private List<Player> _players = new();
    public List<Player> Players => _players;
    public int PlayerNum => _players.Count;

    public Team(TeamSide teamSide)
    {
        side = teamSide;
    }

    public void AddPlayer(Player player)
    {
        _players.Add(player);
    }

    public void SetServe()
    {
        if (_players.Count > 0)
        {
            _players[0].SetServe();
        }
    }
}
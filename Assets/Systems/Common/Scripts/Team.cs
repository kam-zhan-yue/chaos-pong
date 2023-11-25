using System;
using System.Collections.Generic;

[Serializable]
public class Team
{
    private List<Player> _players = new();
    public List<Player> Players => _players;
    public int PlayerNum => _players.Count;

    public void AddPlayer(Player player)
    {
        _players.Add(player);
    }
}
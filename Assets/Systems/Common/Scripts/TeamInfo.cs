using System;
using System.Collections.Generic;

[Serializable]
public class TeamInfo
{
    public PlayerInfo[] players = Array.Empty<PlayerInfo>();

    public TeamInfo()
    {
    }

    public TeamInfo(params PlayerInfo[] players)
    {
        this.players = players;
    }
}
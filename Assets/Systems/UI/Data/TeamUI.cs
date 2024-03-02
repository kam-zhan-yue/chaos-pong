using System.Collections.Generic;

public readonly struct TeamUI
{
    private readonly PlayerUI[] _players;

    public TeamUI(params PlayerUI[] players)
    {
        _players = players;
    }

    public TeamInfo GetTeamInfo(TeamSide teamSide)
    {
        TeamInfo teamInfo = new();
        for (int i = 0; i < _players.Length; ++i)
        {
            teamInfo.players.Add(_players[i].GetPlayerInfo(teamSide));
        }

        return teamInfo;
    }
}
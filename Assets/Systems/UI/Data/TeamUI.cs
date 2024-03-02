using System.Collections.Generic;

public struct TeamUI
{
    public PlayerUI[] players;

    public TeamUI(params PlayerUI[] players)
    {
        this.players = players;
    }
}
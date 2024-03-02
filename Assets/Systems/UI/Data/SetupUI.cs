public struct SetupUI
{
    public readonly TeamInfo redTeam;
    public readonly TeamInfo blueTeam;

    public SetupUI(TeamInfo redTeam, TeamInfo blueTeam)
    {
        this.redTeam = redTeam;
        this.blueTeam = blueTeam;
    }
}
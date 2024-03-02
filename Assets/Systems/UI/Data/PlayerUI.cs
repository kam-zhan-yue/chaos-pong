public struct PlayerUI
{
    public readonly int id;
    public readonly ControlScheme controlScheme;
    private readonly CharacterType _type;
    private Wizard _wizard;

    public PlayerUI(int id, ControlScheme controlScheme, CharacterType type)
    {
        this.id = id;
        this.controlScheme = controlScheme;
        this._type = type;
        _wizard = Wizard.None;
    }

    public void SetWizard(Wizard wizardType)
    {
        this._wizard = wizardType;
    }

    public PlayerInfo GetPlayerInfo(TeamSide teamSide)
    {
        PlayerInfo playerInfo = new();
        playerInfo.identifier = id;
        playerInfo.id = $"{_type} {id}";
        playerInfo.type = _type;
        playerInfo.wizard = _wizard;
        playerInfo.controlScheme = controlScheme;
        playerInfo.teamSide = teamSide;
        return playerInfo;
    }
}
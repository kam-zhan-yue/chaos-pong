public struct PlayerUI
{
    public readonly int id;
    public readonly ControlScheme controlScheme;
    public CharacterConfig config;

    public PlayerUI(int id, ControlScheme controlScheme)
    {
        this.id = id;
        this.controlScheme = controlScheme;
        config = null;
    }

    public void SetConfig(CharacterConfig config)
    {
        this.config = config;
    }
}
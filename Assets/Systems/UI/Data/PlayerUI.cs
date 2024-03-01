public struct PlayerUI
{
    public readonly int id;
    public readonly ControlScheme controlScheme;

    public PlayerUI(int id, ControlScheme controlScheme)
    {
        this.id = id;
        this.controlScheme = controlScheme;
    }
}
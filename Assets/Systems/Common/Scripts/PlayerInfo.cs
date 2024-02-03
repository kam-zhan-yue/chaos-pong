using System;
using Sirenix.OdinInspector;

[Serializable]
public class PlayerInfo
{
    public string id = string.Empty;
    public PlayerType type;
    [HideIf("type", PlayerType.Robot)]
    public ControlScheme controlScheme = ControlScheme.Keyboard;
}

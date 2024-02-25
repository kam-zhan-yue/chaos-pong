using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class PlayerInfo
{
    public int identifier = 0;
    public string id = string.Empty;
    public PlayerType type;
    [HideIf("type", PlayerType.Robot)]
    public ControlScheme controlScheme = ControlScheme.Keyboard;
    [HideInInspector] public TeamSide teamSide;
}

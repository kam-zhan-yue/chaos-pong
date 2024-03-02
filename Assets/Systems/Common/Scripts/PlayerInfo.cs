using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class PlayerInfo
{
    public int identifier = 0;
    public string id = string.Empty;
    public CharacterType type;
    public Wizard wizard;
    [HideIf("type", CharacterType.Robot)]
    public ControlScheme controlScheme = ControlScheme.Keyboard;
    [HideInInspector] public TeamSide teamSide;
}

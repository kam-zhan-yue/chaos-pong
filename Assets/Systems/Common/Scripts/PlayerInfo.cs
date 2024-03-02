using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class PlayerInfo
{
    public int identifier = 0;
    public string id = string.Empty;
    public CharacterType type;
    public CharacterConfig config;
    [HideIf("type", CharacterType.Robot)]
    public ControlScheme controlScheme = ControlScheme.Keyboard;
    [HideInInspector] public TeamSide teamSide;

    public PlayerInfo()
    {
        
    }

    public PlayerInfo(int playerId, ControlScheme controlScheme, CharacterType characterType)
    {
        this.identifier = playerId;
        this.controlScheme = controlScheme;
        this.type = characterType;
    }
}

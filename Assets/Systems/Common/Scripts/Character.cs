using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    private IPlayer[] _playerComponents = Array.Empty<IPlayer>();
    public PlayerInfo PlayerInfo { get; private set; }
    public TeamSide TeamSide => PlayerInfo.teamSide;
    public CharacterState State { get; private set; } = CharacterState.Idle;

    protected virtual void Awake()
    {
        _playerComponents = GetComponents<IPlayer>();
    }
    
    public virtual void Init(PlayerInfo info)
    {
        PlayerInfo = info;
        
        if(!string.IsNullOrEmpty(info.id))
            gameObject.name = info.id;
        
        if (_playerComponents.Length > 0)
        {
            for (int i = 0; i < _playerComponents.Length; ++i)
            {
                _playerComponents[i].InitPlayer(info);
            }
        }
    }

    public void SetState(CharacterState state)
    {
        Debug.Log($"Setting {name} to {state}");
        State = state;
    }
    
    public abstract void SetStart();
}

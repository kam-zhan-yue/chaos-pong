using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private IPlayer[] _playerComponents;

    private void Awake()
    {
        _playerComponents = GetComponents<IPlayer>();
    }
    public void Init(PlayerInfo playerInfo)
    {
        if(!string.IsNullOrEmpty(playerInfo.id))
            gameObject.name = playerInfo.id;
        if (_playerComponents != null)
        {
            for (int i = 0; i < _playerComponents.Length; ++i)
            {
                _playerComponents[i].InitPlayer(playerInfo);
            }
        }
    }
}

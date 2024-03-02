using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Kuroneko.UIDelivery;
using Kuroneko.UtilityDelivery;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerSelectPopup : Popup
{
    [SerializeField] private CharacterSelectPopup characterSelectPopup;
    [SerializeField] private PlayerSelectPopupItem keyboardSelectPopup;
    [SerializeField] private PlayerSelectPopupItem controllerSelectPopup;
    [SerializeField] private PresetController presetController;
    private bool _singlePlayer;
    private UniTaskCompletionSource<SetupUI> _flow = new();
    private PlayerControls _playerControls;

    protected override void InitPopup()
    {
    }

    public void Init(bool singlePlayer)
    {
        _playerControls = new PlayerControls();
        _playerControls.UI.Select.performed += Select;
        _singlePlayer = singlePlayer;
        if (_singlePlayer)
        {
            keyboardSelectPopup.gameObject.SetActiveFast(true);
            controllerSelectPopup.gameObject.SetActiveFast(false);
            keyboardSelectPopup.Init(this, new PlayerInfo(1, ControlScheme.Keyboard, CharacterType.Player));
        }
        else
        {
            keyboardSelectPopup.gameObject.SetActiveFast(true);
            controllerSelectPopup.gameObject.SetActiveFast(true);
            keyboardSelectPopup.Init(this, new PlayerInfo(1, ControlScheme.Keyboard, CharacterType.Player));
            controllerSelectPopup.Init(this, new PlayerInfo(2, ControlScheme.Switch, CharacterType.Player));
        }

        CheckPreset();
        characterSelectPopup.ShowPopup();
    }
    
    public async UniTask<SetupUI> GetFlow()
    {
        _flow = new UniTaskCompletionSource<SetupUI>();
        return await _flow.Task;
    }

    public bool TrySelect(PlayerInfo playerInfo, bool right, out CharacterConfig config)
    {
        return characterSelectPopup.TrySelect(playerInfo, right, out config);
    }

    public bool TrySelectRandom(PlayerInfo playerInfo, out CharacterConfig config)
    {
        return characterSelectPopup.TrySelectRandom(playerInfo, out config);
    }

    public void Deselect(PlayerInfo playerInfo)
    {
        characterSelectPopup.Deselect(playerInfo);
        CheckPreset();
    }

    public void Select(PlayerInfo playerInfo)
    {
        characterSelectPopup.Select(playerInfo);
        CheckPreset();
    }

    public void ReadyUp(PlayerInfo playerInfo)
    {
        characterSelectPopup.ReadyUp(playerInfo);
        CheckPreset();
    }

    private void CheckPreset()
    {
        if (keyboardSelectPopup.State == PlayerSelectPopupItem.PlayerSelectState.Idle &&
            controllerSelectPopup.State == PlayerSelectPopupItem.PlayerSelectState.Idle)
        {
            presetController.SetPresetById("reset");
            return;
        }

        bool ready = AllReady();
        presetController.SetPresetById(ready ? "ready" : "reset");
        if (ready)
        {
            EnableControls();
        }
        else
        {
            DisableControls();
        }
    }

    private bool AllReady()
    {
        bool allReady = false;
        if (_singlePlayer)
        {
            allReady = keyboardSelectPopup.State == PlayerSelectPopupItem.PlayerSelectState.Ready;
        }
        else
        {
            allReady = keyboardSelectPopup.State == PlayerSelectPopupItem.PlayerSelectState.Ready &&
                       controllerSelectPopup.State == PlayerSelectPopupItem.PlayerSelectState.Ready;
        }
        return allReady;
    }

    private void Select(InputAction.CallbackContext callbackContext)
    {
        if (AllReady())
        {
            _flow.TrySetResult(GetSetup());
        }
    }

    private SetupUI GetSetup()
    {
        if (_singlePlayer)
        {
            TeamInfo redTeam = new TeamInfo(keyboardSelectPopup.PlayerInfo);
            TeamInfo blueTeam = new TeamInfo(new PlayerInfo(1, ControlScheme.Keyboard, CharacterType.Trainer));

            return new SetupUI(redTeam, blueTeam);
        }
        else
        {
            TeamInfo redTeam = new TeamInfo(keyboardSelectPopup.PlayerInfo);
            TeamInfo blueTeam = new TeamInfo(controllerSelectPopup.PlayerInfo);
            return new SetupUI(redTeam, blueTeam);
        }
    }

    private void EnableControls()
    {
        _playerControls.Enable();
    }

    private void DisableControls()
    {
        _playerControls.Disable();
    }

    private void OnDestroy()
    {
        if(_playerControls != null)
            _playerControls.Dispose();
    }
}

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
            keyboardSelectPopup.Init(this, new PlayerUI(1, ControlScheme.Keyboard));
        }
        else
        {
            keyboardSelectPopup.gameObject.SetActiveFast(true);
            controllerSelectPopup.gameObject.SetActiveFast(true);
            keyboardSelectPopup.Init(this, new PlayerUI(1, ControlScheme.Keyboard));
            controllerSelectPopup.Init(this, new PlayerUI(2, ControlScheme.Switch));
        }

        CheckPreset();
        characterSelectPopup.ShowPopup();
    }
    
    public async UniTask<SetupUI> GetFlow()
    {
        _flow = new UniTaskCompletionSource<SetupUI>();
        return await _flow.Task;
    }

    public bool TrySelect(PlayerUI playerUI, bool right, out CharacterConfig config)
    {
        return characterSelectPopup.TrySelect(playerUI, right, out config);
    }

    public bool TrySelectRandom(PlayerUI playerUI, out CharacterConfig config)
    {
        return characterSelectPopup.TrySelectRandom(playerUI, out config);
    }

    public void Deselect(PlayerUI playerUI)
    {
        characterSelectPopup.Deselect(playerUI);
        CheckPreset();
    }

    public void Select(PlayerUI playerUI)
    {
        characterSelectPopup.Select(playerUI);
        CheckPreset();
    }

    public void ReadyUp(PlayerUI playerUI)
    {
        characterSelectPopup.ReadyUp(playerUI);
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
            TeamUI redTeam = new TeamUI(keyboardSelectPopup.PlayerUI);

            return new SetupUI(redTeam, new TeamUI());
        }
        else
        {
            TeamUI redTeam = new TeamUI(keyboardSelectPopup.PlayerUI);
            TeamUI blueTeam = new TeamUI(controllerSelectPopup.PlayerUI);
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
}

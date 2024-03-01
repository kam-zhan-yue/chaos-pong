using System.Collections;
using System.Collections.Generic;
using Kuroneko.UtilityDelivery;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSelectPopupItem : MonoBehaviour
{
    private const string KEYBOARD_JOIN = "[Space]";
    private const string SWITCH_JOIN = "[A]";
    
    [SerializeField] private RectTransform joinHolder;
    [SerializeField] private RectTransform playerHolder;
    [SerializeField] private Image playerImage;
    [SerializeField] private TMP_Text joinButtonText;

    private PlayerSelectPopup _playerSelectPopup;
    private PlayerUI _playerUI;
    private PlayerControls _playerControls;
    private PlayerSelectState _state = PlayerSelectState.Idle;

    private enum PlayerSelectState
    {
        Idle = 0,
        Selecting = 1,
        Ready = 2
    }

    public void Init(PlayerSelectPopup playerSelectPopup, PlayerUI playerUI)
    {
        _playerSelectPopup = playerSelectPopup;
        _playerUI = playerUI;
        SetupListeners();
        ShowJoin();
    }
    
    private void SetupListeners()
    {
        _playerControls = new PlayerControls();
        _playerControls.bindingMask = ChaosPongHelper.GetBindingMask(_playerUI.controlScheme);
        _playerControls.UI.Select.performed += Select;
        _playerControls.UI.Deselect.performed += Deselect;
        _playerControls.UI.Navigate.performed += Navigate;
        _playerControls.Enable();
    }

    private void Select(InputAction.CallbackContext callbackContext)
    {
        switch (_state)
        {
            case PlayerSelectState.Idle:
                ShowPlayer();
                ChooseRandom();
                break;
            case PlayerSelectState.Selecting:
                ReadyUp();
                break;
        }
    }

    private void Deselect(InputAction.CallbackContext callbackContext)
    {
        switch (_state)
        {
            case PlayerSelectState.Selecting:
                Deselect();
                ShowJoin();
                break;
            case PlayerSelectState.Ready:
                ShowPlayer();
                break;
        }
    }

    private void Deselect()
    {
        _playerSelectPopup.Deselect(_playerUI);
    }
    
    private void Navigate(InputAction.CallbackContext callbackContext)
    {
        Vector2 movementInput = callbackContext.ReadValue<Vector2>();
        float horizontal = movementInput.x;
        if (horizontal > 0)
        {
            Select(true);
        }
        else if (horizontal < 0)
        {
            Select(false);
        }
    }

    private void Select(bool right)
    {
        if (_playerSelectPopup.TrySelect(_playerUI, right, out CharacterConfig config))
        {
            UpdateConfig(config);
        }
    }

    private void UpdateConfig(CharacterConfig config)
    {
        playerImage.sprite = config.thumbnail;
    }
    
    private void ShowJoin()
    {
        _state = PlayerSelectState.Idle;
        joinHolder.gameObject.SetActiveFast(true);
        playerHolder.gameObject.SetActiveFast(false);
        switch (_playerUI.controlScheme)
        {
            case ControlScheme.Keyboard:
            case ControlScheme.KeyboardSpecial:
                joinButtonText.SetText(KEYBOARD_JOIN);
                break;
            case ControlScheme.Switch:
                joinButtonText.SetText(SWITCH_JOIN);
                break;
            default:
                joinButtonText.SetText(KEYBOARD_JOIN);
                break;
        }
    }

    private void ShowPlayer()
    {
        _state = PlayerSelectState.Selecting;
        joinHolder.gameObject.SetActiveFast(false);
        playerHolder.gameObject.SetActiveFast(true);
    }
    
    private void ChooseRandom()
    {
        if (_playerSelectPopup.TrySelectRandom(_playerUI, out CharacterConfig config))
        {
            UpdateConfig(config);
        }
    }

    private void ReadyUp()
    {
        _state = PlayerSelectState.Ready;
    }
}

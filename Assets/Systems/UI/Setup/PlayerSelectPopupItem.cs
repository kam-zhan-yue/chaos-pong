using System;
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
    [SerializeField] private Image outline;
    [SerializeField] private TMP_Text joinButtonText;

    private PlayerSelectPopup _playerSelectPopup;
    private PlayerControls _playerControls;
    private CharacterConfig _config;
    public PlayerUI PlayerUI { get; private set; }
    public PlayerSelectState State { get; private set; }= PlayerSelectState.Idle;

    [Serializable]
    public enum PlayerSelectState
    {
        Idle = 0,
        Selecting = 1,
        Ready = 2
    }

    public void Init(PlayerSelectPopup playerSelectPopup, PlayerUI playerUI)
    {
        outline.color = Color.white;
        _playerSelectPopup = playerSelectPopup;
        PlayerUI = playerUI;
        SetupListeners();
        ShowJoin();
    }
    
    private void SetupListeners()
    {
        _playerControls = new PlayerControls();
        _playerControls.bindingMask = ChaosPongHelper.GetBindingMask(PlayerUI.controlScheme);
        _playerControls.UI.Select.performed += Select;
        _playerControls.UI.Deselect.performed += Deselect;
        _playerControls.UI.Navigate.performed += Navigate;
        _playerControls.Enable();
    }

    private void Select(InputAction.CallbackContext callbackContext)
    {
        switch (State)
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
        switch (State)
        {
            case PlayerSelectState.Selecting:
                ShowJoin();
                Deselect();
                break;
            case PlayerSelectState.Ready:
                ShowPlayer();
                SelectExisting();
                break;
        }
    }

    private void Deselect()
    {
        _playerSelectPopup.Deselect(PlayerUI);
    }
    
    private void Navigate(InputAction.CallbackContext callbackContext)
    {
        if (State == PlayerSelectState.Selecting)
        {
            Vector2 movementInput = callbackContext.ReadValue<Vector2>();
            float horizontal = movementInput.x;
            Debug.Log(horizontal);
            if (horizontal > ChaosPongHelper.CONTROLLER_NAVIGATE_THRESHOLD)
            {
                Select(true);
            }
            else if (horizontal < -ChaosPongHelper.CONTROLLER_NAVIGATE_THRESHOLD)
            {
                Select(false);
            }
        }
    }

    private void Select(bool right)
    {
        if (_playerSelectPopup.TrySelect(PlayerUI, right, out CharacterConfig config))
        {
            UpdateConfig(config);
        }
    }

    private void UpdateConfig(CharacterConfig config)
    {
        _config = config;
        playerImage.sprite = _config.thumbnail;
    }
    
    private void ShowJoin()
    {
        State = PlayerSelectState.Idle;
        joinHolder.gameObject.SetActiveFast(true);
        playerHolder.gameObject.SetActiveFast(false);
        switch (PlayerUI.controlScheme)
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
        State = PlayerSelectState.Selecting;
        joinHolder.gameObject.SetActiveFast(false);
        playerHolder.gameObject.SetActiveFast(true);
    }
    
    private void ChooseRandom()
    {
        if (_playerSelectPopup.TrySelectRandom(PlayerUI, out CharacterConfig config))
        {
            UpdateConfig(config);
        }
    }

    private void SelectExisting()
    {
        outline.color = Color.white;
        _playerSelectPopup.Select(PlayerUI);
    }

    private void ReadyUp()
    {
        State = PlayerSelectState.Ready;
        _playerSelectPopup.ReadyUp(PlayerUI);
        if (_config)
        {
            outline.color = _config.outline;
        }
    }

    private void OnDestroy()
    {
        _playerControls?.Dispose();
    }
}

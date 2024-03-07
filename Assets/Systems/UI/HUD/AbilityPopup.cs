using System.Collections;
using System.Collections.Generic;
using Kuroneko.UIDelivery;
using Signals;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AbilityPopup : Popup
{
    [SerializeField] private AbilityPopupItem primaryAbility;
    [SerializeField] private AbilityPopupItem secondaryAbility;
    [SerializeField] private AbilityPopupItem specialAbility;
    [SerializeField] private PresetController presetController;
    [SerializeField] private TMP_Text tooltipText;
    [SerializeField] private TMP_Text headerText;
    [SerializeField] private TMP_Text explanationText;
    [SerializeField] private Image explanationBackground;
    private PlayerControls _playerControls;
    private bool _passive = false;
    private PlayerInfo _playerInfo;

    protected override void InitPopup()
    {
        presetController.SetPresetById("reset");
    }

    public void Init(Player player)
    {
        _playerInfo = player.PlayerInfo;
        if (_playerInfo.config)
        {
            primaryAbility.Init(player.PlayerSignal.primarySignal, _playerInfo.config.primaryAbility);
            secondaryAbility.Init(player.PlayerSignal.secondarySignal, _playerInfo.config.secondaryAbility);
            specialAbility.Init(player.PlayerSignal.specialSignal, _playerInfo.config.specialAbility);
        }
        primaryAbility.SetButton(ChaosPongHelper.GetPrimaryButton(_playerInfo.controlScheme));
        secondaryAbility.SetButton(ChaosPongHelper.GetSecondaryButton(_playerInfo.controlScheme));
        specialAbility.SetButton(ChaosPongHelper.GetSpecialButton(_playerInfo.controlScheme));

        string passiveButton = ChaosPongHelper.GetPassiveButton(_playerInfo.controlScheme);
        tooltipText.SetText($"Hold {passiveButton} to view details");
        InitControls();
    }

    private void InitControls()
    {
        _playerControls = new PlayerControls();
        _playerControls.bindingMask = ChaosPongHelper.GetBindingMask(_playerInfo.controlScheme);
        
        _playerControls.Player.Passive.performed += PassivePerformed;
        _playerControls.Player.Passive.canceled += PassiveCancelled;
        
        _playerControls.Player.AbilityPrimary.performed += PrimaryAbility;
        _playerControls.Player.AbilitySecondary.performed += SecondaryAbility;
        _playerControls.Player.AbilitySpecial.performed += SpecialAbility;
        
        _playerControls.Player.AbilityPrimary.canceled += HideExplanation;
        _playerControls.Player.AbilitySecondary.canceled += HideExplanation;
        _playerControls.Player.AbilitySpecial.canceled += HideExplanation;
        _playerControls.Enable();
    }
    private void PassivePerformed(InputAction.CallbackContext callbackContext)
    {
        _passive = true;
    }

    private void PassiveCancelled(InputAction.CallbackContext callbackContext)
    {
        _passive = false;
    }

    private void PrimaryAbility(InputAction.CallbackContext callbackContext)
    {
        if (_passive)
        {
            if (_playerInfo.config && _playerInfo.config.primaryAbility)
            {
                headerText.SetText(_playerInfo.config.primaryAbility.name);
                explanationText.SetText(_playerInfo.config.primaryAbility.description);
                explanationBackground.color = _playerInfo.config.primaryAbility.outline;
            }
            presetController.SetPresetById("show");
        }
    }

    private void SecondaryAbility(InputAction.CallbackContext callbackContext)
    {
        if (_passive)
        {
            if (_playerInfo.config && _playerInfo.config.secondaryAbility)
            {
                headerText.SetText(_playerInfo.config.secondaryAbility.name);
                explanationText.SetText(_playerInfo.config.secondaryAbility.description);
                explanationBackground.color = _playerInfo.config.secondaryAbility.outline;
            }
            presetController.SetPresetById("show");
        }
    }

    private void SpecialAbility(InputAction.CallbackContext callbackContext)
    {
        if (_passive)
        {
            if (_playerInfo.config && _playerInfo.config.specialAbility)
            {
                headerText.SetText(_playerInfo.config.specialAbility.name);
                explanationText.SetText(_playerInfo.config.specialAbility.description);
                explanationBackground.color = _playerInfo.config.specialAbility.outline;
            }
            presetController.SetPresetById("show");
        }
    }

    private void HideExplanation(InputAction.CallbackContext callbackContext)
    {
        presetController.SetPresetById("hide");
    }
}

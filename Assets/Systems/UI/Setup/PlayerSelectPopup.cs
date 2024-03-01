using System.Collections;
using System.Collections.Generic;
using Kuroneko.UIDelivery;
using Kuroneko.UtilityDelivery;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerSelectPopup : Popup
{
    [SerializeField] private CharacterSelectPopup characterSelectPopup;
    [SerializeField] private PlayerSelectPopupItem keyboardSelectPopup;
    [SerializeField] private PlayerSelectPopupItem controllerSelectPopup;
    private bool _singlePlayer;

    protected override void InitPopup()
    {
    }

    public void Init(bool singlePlayer)
    {
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

        characterSelectPopup.ShowPopup();
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
    }
}

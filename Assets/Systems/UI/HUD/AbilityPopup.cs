using System.Collections;
using System.Collections.Generic;
using Kuroneko.UIDelivery;
using Signals;
using UnityEngine;

public class AbilityPopup : Popup
{
    [SerializeField] private AbilityPopupItem primaryAbility;
    [SerializeField] private AbilityPopupItem secondaryAbility;
    [SerializeField] private AbilityPopupItem specialAbility;

    protected override void InitPopup()
    {

    }

    public void Init(Player player)
    {
        primaryAbility.Init(player.PlayerSignal.primarySignal, player.PlayerInfo.config);
        secondaryAbility.Init(player.PlayerSignal.secondarySignal, player.PlayerInfo.config);
        specialAbility.Init(player.PlayerSignal.specialSignal, player.PlayerInfo.config);
        primaryAbility.SetButton(ChaosPongHelper.GetPrimaryButton(player.PlayerInfo.controlScheme));
        secondaryAbility.SetButton(ChaosPongHelper.GetSecondaryButton(player.PlayerInfo.controlScheme));
        specialAbility.SetButton(ChaosPongHelper.GetSpecialButton(player.PlayerInfo.controlScheme));
    }
}

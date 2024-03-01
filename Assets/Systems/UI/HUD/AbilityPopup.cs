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
        primaryAbility.Init(player.PlayerSignal.primarySignal);
        secondaryAbility.Init(player.PlayerSignal.secondarySignal);
        specialAbility.Init(player.PlayerSignal.specialSignal);
    }
}

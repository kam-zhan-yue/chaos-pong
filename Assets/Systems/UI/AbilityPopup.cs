using System.Collections;
using System.Collections.Generic;
using Kuroneko.UIDelivery;
using UnityEngine;

public class AbilityPopup : Popup
{
    [SerializeField] private AbilityPopupItem specialAbility;
    [SerializeField] private AbilityPopupItem primaryAbility;
    [SerializeField] private AbilityPopupItem secondaryAbility;


    protected override void InitPopup()
    {
        
    }
}

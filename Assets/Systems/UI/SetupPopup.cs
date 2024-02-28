using System.Collections;
using System.Collections.Generic;
using Kuroneko.UIDelivery;
using UnityEngine;

public class SetupPopup : Popup
{
    [SerializeField] private ModeSelectPopup modeSelectPopup;
    [SerializeField] private PlayerSelectPopup playerSelectPopup;
    
    protected override void InitPopup()
    {
    }

    public override void ShowPopup()
    {
        base.ShowPopup();
        modeSelectPopup.ShowPopup();
    }
}

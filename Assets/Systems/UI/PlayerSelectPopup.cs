using System.Collections;
using System.Collections.Generic;
using Kuroneko.UIDelivery;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerSelectPopup : Popup
{
    [SerializeField] private PlayerSelectPopupItem keyboardSelectPopup;
    [SerializeField] private PlayerSelectPopup controllerSelect;
    private bool _singlePlayer;
    protected override void InitPopup()
    {
    }

    public void Init(bool singlePlayer)
    {
        _singlePlayer = singlePlayer;
    }
}

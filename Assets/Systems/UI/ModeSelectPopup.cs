using System.Collections;
using System.Collections.Generic;
using Kuroneko.UIDelivery;
using UnityEngine;

public class ModeSelectPopup : Popup
{
    [SerializeField] private RectTransform singlePlayerSelection;
    [SerializeField] private RectTransform twoPlayerSelection;
    
    protected override void InitPopup()
    {
    }
}

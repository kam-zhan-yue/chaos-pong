using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
        SetupFlow().Forget();
    }

    private async UniTask SetupFlow()
    {
        modeSelectPopup.ShowPopup();
        bool singlePlayer = await modeSelectPopup.GetFlow();
        Debug.Log($"Single Player is {singlePlayer}");
        modeSelectPopup.HidePopup();
        playerSelectPopup.ShowPopup();
    }
}

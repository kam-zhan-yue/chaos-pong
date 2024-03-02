using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Kuroneko.UIDelivery;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class SetupPopup : Popup
{
    [SerializeField] private GameSettings gameSettings;
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
        playerSelectPopup.Init(singlePlayer);
        SetupUI setup = await playerSelectPopup.GetFlow();
        Debug.Log($"Setup is: {setup}");
        StartGame(setup);
    }

    private void StartGame(SetupUI setupUI)
    {
        gameSettings.redTeamInfo = setupUI.redTeam;
        gameSettings.blueTeamInfo = setupUI.blueTeam;
        IGameManager gameManager = ServiceLocator.Instance.Get<IGameManager>();
        gameManager?.SetupGame();
        gameManager?.StartGame();
        HidePopup();
    }
}

using System.Collections;
using System.Collections.Generic;
using Kuroneko.UtilityDelivery;
using SuperMaxim.Messaging;
using UnityEngine;

public class UIConnector : MonoBehaviour, IConnectorService
{
    [Header("Popups")]
    [SerializeField] private ScorePopup scorePopup;
    [SerializeField] private TeamPopup redTeamPopup;
    [SerializeField] private TeamPopup blueTeamPopup;
    [SerializeField] private SetupPopup setupPopup;
    
    private void Awake()
    {
        ServiceLocator.Instance.Register<IConnectorService>(this);
        scorePopup.HidePopup();
        redTeamPopup.HidePopup();
        blueTeamPopup.HidePopup();
    }

    public void ShowSetup()
    {
        setupPopup.ShowPopup();
    }

    public void StartGame(GameState gameState)
    {
        scorePopup.StartGame(gameState);
        redTeamPopup.StartGame(gameState, TeamSide.Red);
        blueTeamPopup.StartGame(gameState, TeamSide.Blue);
    }
}

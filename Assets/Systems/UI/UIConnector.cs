using System.Collections;
using System.Collections.Generic;
using Kuroneko.UtilityDelivery;
using SuperMaxim.Messaging;
using UnityEngine;

public class UIConnector : MonoBehaviour, IConnectorService
{
    [SerializeField] private ScorePopup scorePopup;
    [SerializeField] private TeamPopup redTeamPopup;
    [SerializeField] private TeamPopup blueTeamPopup;

    private void Awake()
    {
        ServiceLocator.Instance.Register<IConnectorService>(this);
    }

    public void StartGame(GameState gameState)
    {
        scorePopup.StartGame(gameState);
        redTeamPopup.StartGame(gameState, TeamSide.Red);
        blueTeamPopup.StartGame(gameState, TeamSide.Blue);
    }
}

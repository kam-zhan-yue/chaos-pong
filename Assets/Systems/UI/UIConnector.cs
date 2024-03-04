using System.Collections;
using System.Collections.Generic;
using Kuroneko.UtilityDelivery;
using SuperMaxim.Messaging;
using UnityEngine;

public class UIConnector : MonoBehaviour, IConnectorService
{
    [Header("Popups")]
    [SerializeField] private GamePopup gamePopup;
    [SerializeField] private SetupPopup setupPopup;
    
    private void Awake()
    {
        ServiceLocator.Instance.Register<IConnectorService>(this);
    }

    public void ShowSetup()
    {
        gamePopup.HidePopup();
        setupPopup.ShowPopup();
    }

    public void StartGame(GameState gameState)
    {
        gamePopup.ShowPopup();
        gamePopup.StartGame(gameState);
    }
}

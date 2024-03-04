using Kuroneko.UIDelivery;
using UnityEngine;

public class GamePopup : Popup
{
    [SerializeField] private ScorePopup scorePopup;
    [SerializeField] private TeamPopup redTeamPopup;
    [SerializeField] private TeamPopup blueTeamPopup;
    
    protected override void InitPopup()
    {
        
        
    }

    public void StartGame(GameState gameState)
    {
        scorePopup.StartGame(gameState);
        redTeamPopup.StartGame(gameState, TeamSide.Red);
        blueTeamPopup.StartGame(gameState, TeamSide.Blue);
    }
}
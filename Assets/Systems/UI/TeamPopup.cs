using System.Collections;
using System.Collections.Generic;
using Kuroneko.UIDelivery;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class TeamPopup : Popup
{
    //todo only have support for one player per team for now
    [SerializeField] private AbilityPopup abilityPopup;

    protected override void InitPopup()
    {
        abilityPopup.HidePopup();
    }

    public void StartGame(GameState gameState, TeamSide teamSide)
    {
        Team team = gameState.GetTeam(teamSide);
        List<Player> players = team.Players;
        if (players.Count > 0)
        {
            abilityPopup.ShowPopup();
            abilityPopup.Init(players[0]);
        }
    }
}

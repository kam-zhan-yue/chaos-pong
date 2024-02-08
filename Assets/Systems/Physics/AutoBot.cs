using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoBot : MonoBehaviour, IPlayer
{
    [SerializeField] private TeamSide teamSide;
    [SerializeField] private float height;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Pong pong))
        {
            TeamSide otherSide = ChaosPongHelper.GetOppositeSide(teamSide);
            pong.LaunchAtSide(height, otherSide);
        }
    }

    public void InitPlayer(PlayerInfo playerInfo)
    {
        teamSide = playerInfo.teamSide;
    }
}

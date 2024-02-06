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
            TeamSide otherSide = TeamSide.None;
            if (teamSide == TeamSide.Blue)
                otherSide = TeamSide.Red;
            else if (teamSide == TeamSide.Red)
                otherSide = TeamSide.Blue;
            pong.LaunchAtSide(otherSide, height);
        }
    }

    public void InitPlayer(PlayerInfo playerInfo)
    {
        teamSide = playerInfo.teamSide;
    }
}

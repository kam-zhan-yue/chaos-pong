using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class TeamCamera : MonoBehaviour
{
    public Camera teamCamera;
    public CinemachineTargetGroup targetGroup;

    public void Init(Team team, Rect cameraRect)
    {
        if (team.PlayerNum == 0)
        {
            SetActive(false);
            return;
        }
        SetActive(true);
        teamCamera.rect = cameraRect;
        for (int i = 0; i < team.Players.Count; ++i)
        {
            targetGroup.AddMember(team.Players[i].transform, 1, 2);
        }
    }
    
    public void SetActive(bool active)
    {
        teamCamera.enabled = active;
    }
}

using System;
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
        if (team.PlayerCount() == 0)
        {
            SetActive(false);
            return;
        }
        SetActive(true);
        teamCamera.rect = cameraRect;
        for (int i = 0; i < team.Characters.Count; ++i)
        {
            Type type = team.Characters[i].GetType();
            if (type == typeof(Player))
            {
                targetGroup.AddMember(team.Characters[i].transform, 1, 2);
            }
        }
    }
    
    public void SetActive(bool active)
    {
        teamCamera.enabled = active;
    }
}

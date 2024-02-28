using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class TeamCamera : MonoBehaviour
{
    public Camera teamCamera;
    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineTargetGroup _targetGroup;

    private void Awake()
    {
        _virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        _targetGroup = GetComponentInChildren<CinemachineTargetGroup>();
    }

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
                _targetGroup.AddMember(team.Characters[i].transform, 1, 2);
            }
        }
    }
    
    public void SetActive(bool active)
    {
        teamCamera.enabled = active;
        _virtualCamera.enabled = active;
        _targetGroup.enabled = active;
    }
}

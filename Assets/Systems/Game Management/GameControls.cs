using System.Collections;
using System.Collections.Generic;
using ChaosPong.Common;
using UnityEngine;

public class GameControls : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ServiceLocator.Instance.Get<IGameManager>().RestartGame();
        }
    }
}
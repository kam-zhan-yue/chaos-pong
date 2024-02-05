using System.Collections;
using System.Collections.Generic;
using ChaosPong.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControls : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            // ServiceLocator.Instance.Get<IGameManager>().RestartGame();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

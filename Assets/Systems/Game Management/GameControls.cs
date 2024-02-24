using System.Collections;
using System.Collections.Generic;
using Kuroneko.UtilityDelivery;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControls : MonoBehaviour, IControlsService
{
    private void Awake()
    {
        ServiceLocator.Instance.Register<IControlsService>(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

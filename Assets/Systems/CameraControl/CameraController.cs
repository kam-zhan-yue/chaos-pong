using Kuroneko.UtilityDelivery;
using UnityEngine;

public class CameraController : MonoBehaviour, ICameraService
{
    public Camera genericCamera;
    public TeamCamera redTeam;
    public TeamCamera blueTeam;
    private readonly Rect _leftHalf = new (new Vector2(0f, 0f), new Vector2(0.5f, 1f));
    private readonly Rect _rightHalf = new (new Vector2(0.5f, 0f), new Vector2(0.5f, 1f));
    private readonly Rect _full = new (new Vector2(0f, 0f), new Vector2(1f, 1f));

    private void Awake()
    {
        ServiceLocator.Instance.Register<ICameraService>(this);
    }

    public void ShowSetup()
    {
        genericCamera.enabled = true;
        redTeam.SetActive(false);
        blueTeam.SetActive(false);
    }

    public void SetupGame()
    {
        genericCamera.enabled = false;
        redTeam.SetActive(true);
        blueTeam.SetActive(true);
        Setup();
    }

    private void Setup()
    {
        IGameManager gameManager = ServiceLocator.Instance.Get<IGameManager>();
        if (gameManager.IsMultiCamera())
        {
            redTeam.Init(gameManager.GetRedTeam(), _leftHalf);
            blueTeam.Init(gameManager.GetBlueTeam(), _rightHalf);
        }
        else
        {
            redTeam.Init(gameManager.GetRedTeam(), _full);
            blueTeam.Init(gameManager.GetBlueTeam(), _full);
        }
    }
}

using Kuroneko.UtilityDelivery;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public TeamCamera redTeam;
    public TeamCamera blueTeam;
    private readonly Rect _leftHalf = new (new Vector2(0f, 0f), new Vector2(0.5f, 1f));
    private readonly Rect _rightHalf = new (new Vector2(0.5f, 0f), new Vector2(0.5f, 1f));
    private readonly Rect _full = new (new Vector2(0f, 0f), new Vector2(1f, 1f));

    private void Start()
    {
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

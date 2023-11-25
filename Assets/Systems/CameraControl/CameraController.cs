using System.Collections;
using System.Collections.Generic;
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
        if (ChaosPongManager.instance.MultiCamera)
        {
            redTeam.Init(ChaosPongManager.instance.RedTeam, _leftHalf);
            blueTeam.Init(ChaosPongManager.instance.BlueTeam, _rightHalf);
        }
        else
        {
            redTeam.Init(ChaosPongManager.instance.RedTeam, _full);
            blueTeam.Init(ChaosPongManager.instance.BlueTeam, _full);
        }
    }
}

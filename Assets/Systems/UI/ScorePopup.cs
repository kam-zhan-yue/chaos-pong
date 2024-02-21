using Kuroneko.UIDelivery;
using Signals;
using TMPro;
using UnityEngine;

public class ScorePopup : Popup
{
    [SerializeField] private TMP_Text blueScore;
    [SerializeField] private TMP_Text redScore;
    
    protected override void InitPopup()
    {
        SignalSubscribtionManager manager = new();
        manager.Subscribe(SignalManager.BluePoints, OnBluePointsChanged);
        manager.Subscribe(SignalManager.RedPoints, OnRedPointsChanged);
        manager.Initialize();
    }
    
    private void OnBluePointsChanged(int current)
    {
        blueScore.SetText(current.ToString());
    }

    private void OnRedPointsChanged(int current)
    {
        redScore.SetText(current.ToString());
    }
}

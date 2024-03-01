using Kuroneko.UIDelivery;
using TMPro;
using UnityEngine;

public class ScorePopup : Popup
{
    [SerializeField] private TMP_Text blueScore;
    [SerializeField] private TMP_Text redScore;
    
    protected override void InitPopup()
    {
    }

    public void StartGame(GameState gameState)
    {
        gameState.BluePoints.Subscribe(OnBluePointsChanged);
        gameState.RedPoints.Subscribe(OnRedPointsChanged);
    }
    
    private void OnBluePointsChanged(int prev, int curr)
    {
        blueScore.SetText(curr.ToString());
    }

    private void OnRedPointsChanged(int prev, int curr)
    {
        redScore.SetText(curr.ToString());
    }
}

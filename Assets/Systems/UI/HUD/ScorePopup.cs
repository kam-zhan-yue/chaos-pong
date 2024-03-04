using Kuroneko.UIDelivery;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ScorePopup : Popup
{
    [SerializeField] private TMP_Text blueHeaderScore;
    [SerializeField] private TMP_Text bluePopupScore;
    [SerializeField] private TMP_Text redHeaderScore;
    [SerializeField] private TMP_Text redPopupScore;
    [SerializeField] private PresetController presetController;
    
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
        string score = curr.ToString();
        blueHeaderScore.SetText(score);
        bluePopupScore.SetText(score);
        presetController.SetPresetById("score");
    }

    private void OnRedPointsChanged(int prev, int curr)
    {
        string score = curr.ToString();
        redHeaderScore.SetText(score);
        redPopupScore.SetText(score);
        presetController.SetPresetById("score");
    }
}

using Kuroneko.UIDelivery;
using Kuroneko.UtilityDelivery;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ScorePopup : Popup
{
    [SerializeField] private TMP_Text blueHeaderScore;
    [SerializeField] private TMP_Text bluePopupScore;
    [SerializeField] private TMP_Text redHeaderScore;
    [SerializeField] private TMP_Text redPopupScore;
    [SerializeField] private TMP_Text titlePopup;
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
        OnScore();
    }

    private void OnRedPointsChanged(int prev, int curr)
    {
        string score = curr.ToString();
        redHeaderScore.SetText(score);
        redPopupScore.SetText(score);
        OnScore();
    }

    private void OnScore()
    {
        GameState gameState = ServiceLocator.Instance.Get<IGameManager>()?.GetGameState();
        if (gameState != null)
        {
            TeamSide winner = gameState.GetWinner();
            switch (winner)
            {
                case TeamSide.Red:
                    ShowWinner(gameState.RedTeam);
                    break;
                case TeamSide.Blue:
                    ShowWinner(gameState.BlueTeam);
                    break;
                default:
                    if (gameState.GamePoint())
                    {
                        titlePopup.SetText("Game Point");
                        presetController.SetPresetById("title");
                    }
                    else
                    {
                        presetController.SetPresetById("score");
                    }
                    break;
            }
        }
        else
        {
            presetController.SetPresetById("score");
        }
    }

    private void ShowWinner(Team team)
    {
        if (team.CharacterCount > 0)
        {
            string teamName = team.Characters[0].name;
            titlePopup.SetText($"{teamName} Wins");
            presetController.SetPresetById("title");
        }
    }
}

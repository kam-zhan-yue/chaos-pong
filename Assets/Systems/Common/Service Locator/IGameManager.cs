using Kuroneko.UtilityDelivery;

public interface IGameManager : IGameService
{
    public GameSettings GameSettings();
    public Team GetRedTeam();
    public Team GetBlueTeam();
    public bool IsMultiCamera();
    public void SetupGame();
    public void StartGame();
    public void RestartGame();
    public GameState GetGameState();
}
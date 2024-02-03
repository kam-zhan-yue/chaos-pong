public interface IGameManager : IGameService
{
    public Team GetRedTeam();
    public Team GetBlueTeam();
    public bool IsMultiCamera();
    public void RestartGame();
}
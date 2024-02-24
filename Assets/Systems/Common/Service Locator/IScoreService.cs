using Kuroneko.UtilityDelivery;

public interface IScoreService : IGameService
{
    public void StartGame(GameState gameState);
}
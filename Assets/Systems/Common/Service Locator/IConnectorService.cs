using Kuroneko.UtilityDelivery;

public interface IConnectorService : IGameService
{
    public void StartGame(GameState gameState);
}
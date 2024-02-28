using Kuroneko.UtilityDelivery;

public interface IConnectorService : IGameService
{
    public void ShowSetup();
    public void StartGame(GameState gameState);
}
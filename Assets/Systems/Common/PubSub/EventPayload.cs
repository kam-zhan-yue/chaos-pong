public class EventPayload
{
    public readonly GameEvent gameEvent;

    public EventPayload(GameEvent gameEvent)
    {
        this.gameEvent = gameEvent;
    }
}
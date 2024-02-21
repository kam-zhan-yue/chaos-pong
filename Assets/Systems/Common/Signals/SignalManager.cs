using Signals;

public static class SignalManager
{
    public static readonly IntToIntSignal BluePoints = new IntToIntSignal(new Signal<int>());
    public static readonly IntToIntSignal RedPoints = new IntToIntSignal(new Signal<int>());
}

using Signals;

public class IntToIntSignal : ComputedSignal<int, int>
{
    public IntToIntSignal(ISignal<int> signal) : base(signal) { }
    protected override int Compute(int value)
    {
        return value;
    }
}
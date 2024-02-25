using Signals;

public class BoolToBoolSignal : ComputedSignal<bool, bool>
{
    public BoolToBoolSignal(ISignal<bool> signal) : base(signal) { }
    protected override bool Compute(bool value)
    {
        return value;
    }
}
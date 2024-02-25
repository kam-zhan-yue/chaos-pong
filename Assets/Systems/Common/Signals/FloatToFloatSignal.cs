using Signals;

public class FloatToFloatSignal : ComputedSignal<float, float>
{
    public FloatToFloatSignal(ISignal<float> signal) : base(signal) { }
    protected override float Compute(float value)
    {
        return value;
    }
}
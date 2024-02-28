using System;

[Serializable]
public class PongModifier
{
    public bool deadly;

    public void Reset()
    {
        deadly = false;
    }
}
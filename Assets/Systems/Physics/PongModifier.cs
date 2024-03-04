using System;

[Serializable]
public class PongModifier
{
    public bool deadly;
    public float timeScale = ChaosPongPhysics.DEFAULT_TIME_SCALE;

    public void Reset()
    {
        deadly = false;
        timeScale = ChaosPongPhysics.DEFAULT_TIME_SCALE;
    }
}
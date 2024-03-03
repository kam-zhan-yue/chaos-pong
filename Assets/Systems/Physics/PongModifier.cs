using System;

[Serializable]
public class PongModifier
{
    public bool deadly;
    public float timeScale = 1f;

    public void Reset()
    {
        deadly = false;
        timeScale = 1f;
    }
}
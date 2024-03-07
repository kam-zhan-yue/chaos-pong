 using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Ability Config")]
public class AbilityConfig : ScriptableObject
{
    public Color outline;
    public Sprite thumbnail;
    [TextArea]
    public string description;
}

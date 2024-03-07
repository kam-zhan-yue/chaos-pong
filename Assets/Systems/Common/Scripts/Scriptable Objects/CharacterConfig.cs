using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Character Config")]
[InlineEditor()]
public class CharacterConfig : ScriptableObject
{
    public Player player;
    public Sprite thumbnail;
    public Color outline;
    public AbilityConfig specialAbility;
    public AbilityConfig primaryAbility;
    public AbilityConfig secondaryAbility;
}
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Character Config")]
public class CharacterConfig : ScriptableObject
{
    public Player player;
    public Wizard wizard;
    public Sprite thumbnail;
    public Color outline;
}
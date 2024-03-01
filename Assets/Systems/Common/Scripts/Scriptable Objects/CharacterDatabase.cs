using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Character Database")]
public class CharacterDatabase : ScriptableObject
{
    public CharacterConfig[] characters = Array.Empty<CharacterConfig>();
}
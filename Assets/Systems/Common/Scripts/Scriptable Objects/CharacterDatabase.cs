using System;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Character Database")]
public class CharacterDatabase : ScriptableObject
{
    [TableList]
    public CharacterConfig[] characters = Array.Empty<CharacterConfig>();
}